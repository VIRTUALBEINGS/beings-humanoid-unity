using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.UnityIntegration;
using VirtualBeings.Tech.Utils;
using static System.TimeZoneInfo;

namespace VirtualBeings.Beings.Humanoid
{
    [CustomEditor(typeof(HumanoidSharedClientSettings))]
    public partial class HumanoidSharedSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            HumanoidSharedClientSettings settings = (HumanoidSharedClientSettings)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Update Humanoid animation data", GUILayout.Width(255)))
            {
                EditorUtility.SetDirty(settings);

                OnInspectorGUI_InitActuatorData(settings);
                OnInspectorGUI_InitActuatorData_FSS(settings);

                AssetDatabase.SaveAssets();
            }
        }

        private void InitRSTransitionMatrix(
            int nRS,
            RStoRSMatrixEntry[] _RStoRSMatrix,
            List<Triple<RSHumanoid, RSHumanoid, RSHumanoid>> listOverrides
        )
        {
            // apply direct overrides
            foreach (Triple<RSHumanoid, RSHumanoid, RSHumanoid> triple in listOverrides)
                _RStoRSMatrix[triple.First.ID * nRS + triple.Second.ID].Next = triple.Third.ID;

            // now the tricky part: for each matrix element that is still RSHumanoid.None, try to find the 'best'
            // intermediate state which allows reaching the target state with as few transitions as possible;
            // we have to do this using iterative deepening, where subsequent iterations can profit from
            // the non-direct transitions that were found in previous ones
            int RS_None = RSHumanoid.None.ID;
            bool newTransitionFound;
            RStoRSMatrixEntry[] RStoRSMatrix_tmp = new RStoRSMatrixEntry[nRS * nRS]; // explained below

            // now do iterative deepening as long as we keep finding new transitions (set up to be guaranteed to terminate)
            do
            {
                newTransitionFound = false;

                // reset temporary RS matrix
                for (int i = 0; i < nRS * nRS; i++)
                    RStoRSMatrix_tmp[i].Next = RS_None;

                for (int i = 0; i < nRS * nRS; i++)
                {
                    if (_RStoRSMatrix[i].Next == RS_None)
                    {
                        int fromRS = i / nRS;
                        int toRS = i % nRS;
                        int nextRS;

                        if (fromRS != RS_None &&
                            toRS != RS_None &&
                            fromRS != toRS &&
                            (nextRS = BestNextState(fromRS, toRS)) != RS_None)
                        {
                            // crucially, we don't save results directly to _RStoRSMatrix but only to RStoRSMatrix_tmp
                            RStoRSMatrix_tmp[i].Next = nextRS;
                            newTransitionFound = true;
                        }
                    }
                }

                // now collect all results of this iterative deepening step and save them to _RStoRSMatrix; this
                // approach guarantees that at each iteration, transitions with the smallest number of connections between
                // from and to are privileged, irrespective of which transitions where found right before during the same iteration
                if (newTransitionFound)
                    for (int i = 0; i < nRS * nRS; i++)
                        if (RStoRSMatrix_tmp[i].Next != RS_None)
                            _RStoRSMatrix[i].Next = RStoRSMatrix_tmp[i].Next;
            }
            while (newTransitionFound);

            // ---------------------------------------
            // checks
            for (int i = 0; i < nRS * nRS; i++)
            {
                int fromRS = (i / nRS);
                int toRS = (i % nRS);

                if (fromRS != toRS &&
                    fromRS != RS_None &&
                    toRS != RS_None)
                {
                    // check 1: see if there remain unreachable states
                    if (_RStoRSMatrix[i].Next == RS_None)
                    {
                        Debug.LogError(
                            "InitRSTransitionMatrix can't resolve transition " + i + ": " + fromRS + " to " + toRS
                        );
                    }
                    else
                    {
                        // check 2: make sure every state reaches every other state within MaxSteps
                        const int MaxSteps = 4;
                        bool bFound = false;
                        int nextRS = _RStoRSMatrix[i].Next;

                        for (int n = 1; n <= MaxSteps; n++)
                        {
                            if (nextRS == toRS)
                            {
                                bFound = true;
                                break;
                            }

                            nextRS = _RStoRSMatrix[(int)nextRS * nRS + (int)toRS].Next;
                        }

                        if (!bFound)
                            Debug.LogError(
                                "InitRSTransitionMatrix error: " + fromRS + " cannot reach " + toRS +
                                " within a maximum of #" + MaxSteps + " transitions."
                            );
                    }
                }
            }

            // local function
            int BestNextState(int fromRS, int toRS)
            {
                // go through all states reachable by fromRS and return the first one that has a transition to toRS.
                for (int j = 0; j < nRS; j++)
                {
                    int nextRS = (int)_RStoRSMatrix[fromRS * nRS + j].Next;

                    if (fromRS != j && nextRS != RS_None &&
                        _RStoRSMatrix[nextRS * nRS + toRS].Next != RSHumanoid.None.ID)
                        return nextRS;
                }

                // fail
                return RS_None;
            }
        }

        private void OnInspectorGUI_InitActuatorData(HumanoidSharedClientSettings sharedSettings)
        {
            // ---------------------------------------
            int nRS = RSHumanoid.All.Count;

            int RS_None               = RSHumanoid.None.ID;
            int RS_Stand              = RSHumanoid.Stand.ID;
            int RS_Sit                = RSHumanoid.Sit.ID;
            int RS_Walk               = RSHumanoid.Walk.ID;
            int RS_Jog                = RSHumanoid.Jog.ID;
            int RS_Dance_LowEnergy    = RSHumanoid.Dance_LowEnergy.ID;
            int RS_Dance_Robot        = RSHumanoid.Dance_Robot.ID;
            int RS_Dance_Wave         = RSHumanoid.Dance_Wave.ID;
            int RS_Dance_Samba        = RSHumanoid.Dance_Samba.ID;
            int RS_Dance_GangnamStyle = RSHumanoid.Dance_GangnamStyle.ID;

            // ---------------------------------------
            RSInfo[] RSInfos = new RSInfo[nRS];
            List<STTransitionInfo> STTransitionInfosL = new();

            int AddS(string name, float crossFadeDuration = 0f, float transitionOffset = 0f, bool isUninterruptible = false,
            float exitTime = -1f, int nextExitTimeVariation = -1, Easings.Functions suppress = Easings.Functions.Constant0,
            float maxCrossFadeDurationOfTransitionParameters = -1f)
            {
                STTransitionInfo st = new(
                    Animator.StringToHash(name),
                    name,
                    crossFadeDuration,
                    transitionOffset,
                    isUninterruptible,
                    exitTime,
                    nextExitTimeVariation,
                    suppress,
                    maxCrossFadeDurationOfTransitionParameters
                );
                STTransitionInfosL.Add(st);
                return STTransitionInfosL.Count - 1;
            }

            float STCrossfadeTime = sharedSettings.DefaultCrossFadeTimeForSTs;

            // NB: unnecessary to add RSInfo for RSHumanoid.None (why? because unity deserialization
            // instantiates null fields of custom classes and because RSInfo implements ISerializationCallbackReceiver)
            // OBS: each of the Animator states referenced here must *exist* and have the *tag* 'TransitionBehavior' (checked automatically below)

            RSInfos[RS_Stand] = new RSInfo(
                Animator.StringToHash("Stand"),
                Animator.StringToHash("BA_State_Stand"),
                new int[]
                {
                    BodyAttitudeHumanoid.Idle.ID,
                    BodyAttitudeHumanoid.Lean.ID,
                    BodyAttitudeHumanoid.Angry.ID,
                    BodyAttitudeHumanoid.Confused.ID,
                    BodyAttitudeHumanoid.Contrapposto.ID,
                    BodyAttitudeHumanoid.Defeated.ID,
                    BodyAttitudeHumanoid.Disgusted.ID,
                    BodyAttitudeHumanoid.Judging.ID,
                    BodyAttitudeHumanoid.LegJoined.ID,
                    BodyAttitudeHumanoid.LegSpread.ID,
                    BodyAttitudeHumanoid.Relaxed.ID,
                    BodyAttitudeHumanoid.Scared.ID,
                    BodyAttitudeHumanoid.HandsBehindBack.ID,
                    BodyAttitudeHumanoid.HandsOnHips.ID,
                },
                new RSInfo.ST_and_int[]
                {
                    new(
                        STHumanoid.Neutral.ID,
                        AddS("ST_Stand_Neutral", maxCrossFadeDurationOfTransitionParameters: 0.1f)
                    ),

                    new(STHumanoid.Turn.ID, AddS("ST_Stand_Turn", maxCrossFadeDurationOfTransitionParameters: 0f)),
                    new(STHumanoid.StepL.ID, AddS("ST_Stand_StepL", maxCrossFadeDurationOfTransitionParameters: 0f)),
                    new(STHumanoid.StepR.ID, AddS("ST_Stand_StepR", maxCrossFadeDurationOfTransitionParameters: 0f)),

                    new(STHumanoid.Defeated.ID, AddS("ST_Stand_Defeated", STCrossfadeTime)),
                    new(STHumanoid.GreetActive.ID, AddS("ST_Stand_Greet_Active", STCrossfadeTime)),
                    new(STHumanoid.Greet.ID, AddS("ST_Stand_Greet", STCrossfadeTime)),
                    new(STHumanoid.Reacting.ID, AddS("ST_Stand_Reacting", STCrossfadeTime)),
                    new(STHumanoid.Angry_Gesture.ID, AddS("ST_Stand_AngryGesture", STCrossfadeTime)),
                    new(STHumanoid.Angry_CrossArms.ID, AddS("ST_Stand_Angry_CrossArms", STCrossfadeTime)),
                    new(STHumanoid.StretchArms.ID, AddS("ST_Stand_StretchArms", STCrossfadeTime)),
                    new(STHumanoid.InspectCurious.ID, AddS("ST_Stand_Inspect_Curious", STCrossfadeTime)),
                    new(STHumanoid.Scared.ID, AddS("ST_Stand_Scared", STCrossfadeTime)),
                    new(STHumanoid.ScaredLeftRight.ID, AddS("ST_Stand_Scared_Looking_Around", STCrossfadeTime)),
                    new(STHumanoid.Laugh.ID, AddS("ST_Stand_Laugh", STCrossfadeTime)),
                    new(STHumanoid.Applause.ID, AddS("ST_Stand_Applause", STCrossfadeTime)),
                    new(STHumanoid.Disappointed.ID, AddS("ST_Stand_Disappointed", STCrossfadeTime)),
                    new(STHumanoid.Shrug.ID, AddS("ST_Stand_Shrug", STCrossfadeTime)),
                    new(STHumanoid.SwingArms_Subtle.ID, AddS("ST_Stand_SwingArms_Subtle", STCrossfadeTime)),
                    new(STHumanoid.HeadTilt_Stretch.ID, AddS("ST_Stand_HeadTilt_Stretch", STCrossfadeTime)),
                    new(STHumanoid.RollShoulder.ID, AddS("ST_Stand_RollShoulder", STCrossfadeTime)),
                    new(STHumanoid.SatisfiedJumpy.ID, AddS("ST_Stand_Satisfied_Jumpy", STCrossfadeTime)),
                    new(STHumanoid.TwistTorso_Subtle.ID, AddS("ST_Stand_TwistTorso_Subtle", STCrossfadeTime)),
                    new(STHumanoid.Sad_FootKick.ID, AddS("ST_Stand_Sad_FootKick", STCrossfadeTime)),
                    new(STHumanoid.HandsOnBack.ID, AddS("ST_Stand_HandsOnBack", STCrossfadeTime)),
                    new(STHumanoid.HandsOnBackExit.ID, AddS("ST_Stand_HandsOnBackExit", STCrossfadeTime, 0.5f)),
                    new(STHumanoid.StretchArmsL.ID, AddS("ST_Stand_StretchLeftArms", STCrossfadeTime)),
                    new(STHumanoid.StretchArmsR.ID, AddS("ST_Stand_StretchRightArms", STCrossfadeTime)),
                    new(STHumanoid.StretchArmsR.ID, AddS("ST_Stand_StretchRightArms", STCrossfadeTime)),
                    new(STHumanoid.StretchArmsR.ID, AddS("ST_Stand_StretchRightArms", STCrossfadeTime)),
                    new(STHumanoid.InspectCuriousL.ID, AddS("ST_Stand_Inspect_Curious_L", STCrossfadeTime)),
                    new(STHumanoid.InspectCuriousR.ID, AddS("ST_Stand_Inspect_Curious_R", STCrossfadeTime)),
                    new(STHumanoid.ShakeFootL.ID, AddS("ST_Stand_ShakeFootL", STCrossfadeTime)),
                    new(STHumanoid.ShakeFootR.ID, AddS("ST_Stand_ShakeFootR", STCrossfadeTime)),
                    new(STHumanoid.Inspect_Arms_WideOpen.ID, AddS("ST_Stand_Inspect_Arms_WideOpen", STCrossfadeTime)),
                    new(STHumanoid.BalanceAdjust.ID, AddS("ST_Stand_BalanceAdjust", STCrossfadeTime)),

                    new(
                        STHumanoid.Speaking_Long_Overt_Agreeing_RH.ID,
                        AddS("ST_Stand_Speaking_Long_Overt_Agreeing_RightHand", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Long_Overt_Explaining_BH.ID,
                        AddS("ST_Stand_Speaking_Long_Overt_BothHands_Explaining", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Long_Overt_Left_BH.ID,
                        AddS("ST_Stand_Speaking_Long_Overt_BothHands_Left", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Long_Overt_Explaining_Slow.ID,
                        AddS("ST_Stand_Speaking_Long_Overt_Explaining_Slow", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Med_Overt_Argument_BH.ID,
                        AddS("ST_Stand_Speaking_MidLength_Overt_BothHands_Argument", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Med_Overt_Explaining_BH.ID,
                        AddS("ST_Stand_Speaking_MidLength_Overt_BothHands_Explaining", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Med_Overt_Sigh_RH.ID,
                        AddS("ST_Stand_Speaking_MidLength_Overt_RightHand_Sigh", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Med_Subtle_Argument_RH.ID,
                        AddS("ST_Stand_Speaking_MidLength_Subtle_RightHand_Argument", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Short_Overt_Agreeing_BH.ID,
                        AddS("ST_Stand_Speaking_Short_Overt_BothHands_Agreeing", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Short_Overt_High_BH.ID,
                        AddS("ST_Stand_Speaking_Short_Overt_BothHands_High", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Short_Overt_Agreeing_RH.ID,
                        AddS("ST_Stand_Speaking_Short_Overt_RightHand_Agree", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Short_Subtle_Agreeing_BH.ID,
                        AddS("ST_Stand_Speaking_Short_Subtle_BothHands_Agreeing", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Short_Subtle_ArmsOpen_BH.ID,
                        AddS("ST_Stand_Speaking_Short_Subtle_BothHands_ArmsOpen", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Short_Subtle_Brief_RH.ID,
                        AddS("ST_Stand_Speaking_Short_Subtle_RightHand_Brief", STCrossfadeTime)
                    ),
                    new(
                        STHumanoid.Speaking_Short_Subtle_Twist_RH.ID,
                        AddS("ST_Stand_Speaking_Short_Subtle_RightHand_Twist", STCrossfadeTime)
                    ),

                    // STC
                    new(STHumanoid.HandsOnBackCycle.ID, AddS("STC_Stand_HandsOnBack", .5f)),
                    new(STHumanoid.Idle.ID, AddS("STC_Stand_Idle", 1.2f)),
                }
            );

            RSInfos[RS_Walk] = new RSInfo(
                Animator.StringToHash("Root_Walk"),
                Animator.StringToHash("BA_State_Stand"),
                new int[] { },
                new RSInfo.ST_and_int[] { }
            );

            RSInfos[RS_Sit] = new RSInfo(
                Animator.StringToHash("Root_Sit"),
                Animator.StringToHash("BA_State_Stand"),
                new int[] { },
                new RSInfo.ST_and_int[] { }
            );

            RSInfos[RS_Jog] = new RSInfo(
               Animator.StringToHash("Root_Jog"),
               Animator.StringToHash("BA_State_Stand"),
               new int[] { },
               new RSInfo.ST_and_int[] { }
           );

            RSInfos[RS_Dance_LowEnergy] = new RSInfo(
                Animator.StringToHash("Root_Dance_LowEnergy"),
                Animator.StringToHash("BA_State_Stand"),
                new int[] { },
                new RSInfo.ST_and_int[] { }
            );

            RSInfos[RS_Dance_Robot] = new RSInfo(
                Animator.StringToHash("Root_Dance_Robot"),
                Animator.StringToHash("BA_State_Stand"),
                new int[] { },
                new RSInfo.ST_and_int[] { }
            );

            RSInfos[RS_Dance_Wave] = new RSInfo(
                Animator.StringToHash("Root_Dance_Wave"),
                Animator.StringToHash("BA_State_Stand"),
                new int[] { },
                new RSInfo.ST_and_int[] { }
            );

            RSInfos[RS_Dance_Samba] = new RSInfo(
                Animator.StringToHash("Root_Dance_Samba"),
                Animator.StringToHash("BA_State_Stand"),
                new int[] { },
                new RSInfo.ST_and_int[] { }
            );

            RSInfos[RS_Dance_GangnamStyle] = new RSInfo(
                Animator.StringToHash("Root_Dance_GangNamStyle"),
                Animator.StringToHash("BA_State_Stand"),
                new int[] { },
                new RSInfo.ST_and_int[] { }
            );

            STTransitionInfo[] STTransitionInfos = new STTransitionInfo[STTransitionInfosL.Count];
            for (int i = 0; i < STTransitionInfosL.Count; i++)
            {
                STTransitionInfos[i] = STTransitionInfosL[i];
            }

            // ---------------------------------------

            RStoRSMatrixEntry[] RStoRSMatrix = new RStoRSMatrixEntry[nRS * nRS];
            List<RSTransitionInfo> RSTransitionInfosL = new();

            int AddP(RSTransitionInfo transitionInfo)
            {
                RSTransitionInfosL.Add(transitionInfo);
                return RSTransitionInfosL.Count - 1;
            }

            int AddOT(
                string ot,
                int transitionType = 0,
                float crossFadeDuration = 0f,
                float transitionOffset = 0f,
                float exitTime = -1f,
                int nextExpressiveVariation = -1,
                int nextExitTimeVariation = -1,
                float maxDistance = 0f,
                float maxAngle = 180f,
                float maxCrossFadeDurationOfTransitionParameters = -1f
            )
            {
                return AddP(
                    new RSTransitionInfo(
                        Animator.StringToHash(ot),
                        transitionType,
                        crossFadeDuration,
                        transitionOffset,
                        exitTime,
                        nextExpressiveVariation,
                        nextExitTimeVariation,
                        maxDistance,
                        maxAngle,
                        maxCrossFadeDurationOfTransitionParameters
                    )
                );
            }

            float RSInterruptionTime = sharedSettings.CrossFadeTimeForRSInterruption;

            for (int i = 0; i < RStoRSMatrix.Length; i++)
            {
                RStoRSMatrix[i] = new RStoRSMatrixEntry(RSHumanoid.None.ID, -1);
            }

            // NB: none of the entries below initialize RStoRSMatrixEntry.Variations because it can't be serialized;
            // instead Variations is reconstructed from the linked list of 'RSTransitionInfo.NextExpressiveVariation's in BirdBeing

            int AddDefaultTransition(int RS, float crossFadeDuration) =>
                AddP(
                    new RSTransitionInfo(RSInfos[RS].StateHash, TransitionTypeHumanoid.Default.ID,
                        crossFadeDuration >= 0f ? crossFadeDuration : RSInterruptionTime)
                );

            RStoRSMatrixEntry DefaultTransition(int RS, float crossfadeDuration) =>
                new(RS, AddDefaultTransition(RS, crossfadeDuration));

            RStoRSMatrixEntry NamedTransition(int RS, string OT, float crossFadeDuration = -1f)
            {
                return new RStoRSMatrixEntry(
                    RS,
                    AddOT(
                        OT,
                        TransitionTypeHumanoid.Default.ID,
                        crossFadeDuration > 0 ? crossFadeDuration : RSInterruptionTime
                    )
                );
            }

            void InsertDefaultOT(int to, int from, float crossFadeDuration = -1f)
            {
                RStoRSMatrix[from * nRS + to] = DefaultTransition(to, crossFadeDuration);
            }

            void InsertNamedOT(int to, int from, string transitionName, float crossFadeDuration = -1f)
            {
                RStoRSMatrix[from * nRS + to] = NamedTransition(to, transitionName, crossFadeDuration);
            }

            // ---------------------------------------
            // -> Stand
            InsertDefaultOT(RS_Stand, RS_Stand);
            InsertDefaultOT(RS_Stand, RS_Sit);
            InsertDefaultOT(RS_Stand, RS_Jog);

            RStoRSMatrix[RS_Walk * nRS + RS_Stand] = new RStoRSMatrixEntry(
                RS_Stand,
                AddP(
                    new RSTransitionInfo(
                        RSInfos[RS_Stand].StateHash,
                        TransitionTypeHumanoid.Default.ID,
                        RSInterruptionTime,
                        nextExpressiveVariation:
                        AddOT(
                            "Walk_to_Stand_Left",
                            TransitionTypeHumanoid.NiceStop.ID,
                            transitionOffset: 0f,
                            exitTime: 0.5f,
                            maxDistance: 0.5f,
                            crossFadeDuration: 0.2f,
                            nextExitTimeVariation:
                            AddOT(
                                "Walk_to_Stand_Right",
                                TransitionTypeHumanoid.NiceStop.ID,
                                transitionOffset: 0f,
                                maxDistance: 0.5f,
                                exitTime: 0f,
                                crossFadeDuration: 0.2f
                            )
                        )
                    )
                )
            );

            //InsertNamedOT(RS_Stand, RS_Dance_LowEnergy, "Dance_LowEnergy_to_Stand");
            //InsertNamedOT(RS_Stand, RS_Dance_Robot, "Dance_Robot_to_Stand");
            //InsertNamedOT(RS_Stand, RS_Dance_Wave, "Dance_Wave_to_Stand");
            //InsertNamedOT(RS_Stand, RS_Dance_Samba, "Dance_Samba_to_Stand");
            //InsertNamedOT(RS_Stand, RS_Dance_GangnamStyle, "Dance_GangNamStyle_to_Stand");

            InsertNamedOT(RS_Stand, RS_Dance_LowEnergy, "Stand", .05f);
            InsertNamedOT(RS_Stand, RS_Dance_Robot, "Stand", .05f);
            InsertNamedOT(RS_Stand, RS_Dance_Wave, "Stand", .05f);
            InsertNamedOT(RS_Stand, RS_Dance_Samba, "Stand", .04f);
            InsertNamedOT(RS_Stand, RS_Dance_GangnamStyle, "Stand", .05f);

            // ---------------------------------------
            // -> Walk
            InsertDefaultOT(RS_Walk, RS_Walk);
            RStoRSMatrix[RS_Stand * nRS + RS_Walk] = new RStoRSMatrixEntry(
                RS_Walk,
                AddOT(
                    "Stand_to_WalkL",
                    TransitionTypeHumanoid.DefaultLeft.ID,
                    nextExpressiveVariation: AddOT(
                        "Stand_to_WalkR",
                        TransitionTypeHumanoid.DefaultRight.ID,
                        nextExpressiveVariation: AddOT(
                            "Stand_to_Walk_BackwardsL",
                            TransitionTypeHumanoid.DefaultBackwardLeft.ID,
                            nextExpressiveVariation: AddOT(
                                "Stand_to_Walk_BackwardsR",
                                TransitionTypeHumanoid.DefaultBackwardRight.ID
                            )
                            // nextExpressiveVariation: AddP(new RSTransitionInfo(Animator.StringToHash("Stand_to_Walk_AtAngleL"), TransitionTypeHumanoid.AtAngleLeft.ID,
                            // nextExpressiveVariation: AddP(new RSTransitionInfo(Animator.StringToHash("Stand_to_Walk_AtAngleR"), TransitionTypeHumanoid.AtAngleRight.ID)))))))));
                        )
                    )
                )
            );

            // ------------------------------
            // -> Dances
            InsertNamedOT(RS_Dance_LowEnergy, RS_Stand, "Stand_to_Dance_LowEnergy");
            InsertNamedOT(RS_Dance_Robot, RS_Stand, "Stand_to_Dance_Robot");
            InsertNamedOT(RS_Dance_Wave, RS_Stand, "Stand_to_Dance_Wave");
            InsertNamedOT(RS_Dance_Samba, RS_Stand, "Stand_to_Dance_Samba");
            InsertNamedOT(RS_Dance_GangnamStyle, RS_Stand, "Stand_to_Dance_GangNamStyle");

            // ------------------------------
            // -> Sit
            InsertDefaultOT(RS_Sit, RS_Sit);
            InsertDefaultOT(RS_Sit, RS_Stand);
            InsertDefaultOT(RS_Sit, RS_Walk);

            // ------------------------------
            // -> Jog
            InsertDefaultOT(RS_Jog, RS_Jog);
            RStoRSMatrix[RS_Stand * nRS + RS_Jog] = new RStoRSMatrixEntry(
                RS_Jog,
                AddOT(
                    "Stand_to_JogL",
                    TransitionTypeHumanoid.DefaultLeft.ID,
                    nextExpressiveVariation: AddOT(
                        "Stand_to_JogR",
                        TransitionTypeHumanoid.DefaultRight.ID
                    )
                )
            );


            // RStoRSMatrix[RS_Hover * nRS + RS_Stand] = new RStoRSMatrixEntry(
            // RSHumanoid.Stand.ID,
            // AddP(
            // new RSTransitionInfo(Animator.StringToHash("Hover_to_Stand_Landing"), TransitionTypeHumanoid.Default.ID)
            // )
            // );

            // ---------------------------------------
            // necessary to force transitions that can't be found with current algorithm
            List<Triple<RSHumanoid, RSHumanoid, RSHumanoid>> listOverrides =
                new()
                {
                    //new Triple<RSHumanoid, RSHumanoid, RSHumanoid>(RSHumanoid.Stand, RSHumanoid.LieRelaxedR, RSHumanoid.LieAwareR),

                    // format: first == from (row), second == to (column), third == next (matrix entry)
                };

            InitRSTransitionMatrix(nRS, RStoRSMatrix, listOverrides);
            DebugShowRSTransitionMatrix(RStoRSMatrix);

            // copy List(T) into array
            RSTransitionInfo[] RSTransitionInfos = new RSTransitionInfo[RSTransitionInfosL.Count];
            for (int i = 0; i < RSTransitionInfosL.Count; i++)
                RSTransitionInfos[i] = RSTransitionInfosL[i];

            // ---------------------------------------
            // check that all referenced OTs and STs exist and are correctly tagged
            string[] animatorControllerGuid = AssetDatabase.FindAssets(sharedSettings.AnimatorController.name);
            AnimatorController controller = (AnimatorController)AssetDatabase.LoadAssetAtPath(
                AssetDatabase.GUIDToAssetPath(animatorControllerGuid[0]),
                typeof(AnimatorController)
            );
            ChildAnimatorState[] states = controller.layers[0].stateMachine.states;

            foreach (RSTransitionInfo info in RSTransitionInfosL)
            {
                bool bFound = states.Select(t => t.state).Any(state => state.nameHash == info.Hash);

                // NB: if this returns an error, note down the Hash and set a conditional breakpoint in
                // the constructor of RSTransitionInfo in BirdAnimStates.cs in Rascal
                if (!bFound)
                {
                    Debug.LogError("Error checking OT #" + info.Hash);
                }
            }

            foreach (STTransitionInfo info in STTransitionInfosL)
            {
                bool found = false;
                bool hasTransitionTag = false;

                foreach (ChildAnimatorState t in states)
                {
                    AnimatorState state = t.state;

                    if (state.nameHash == info.Hash)
                    {
                        found = true;
                        hasTransitionTag = state.tag == "TransitionBehavior";
                        break;
                    }
                }

                if (!found)
                {
                    Debug.LogError($"Error checking ST #{info.Name}; not found");
                }
                else if (!hasTransitionTag)
                {
                    Debug.LogError($"Error checking ST #{info.Name}; found but transition tag not present");
                }
            }

            USTInfo[] USTInfos              = new USTInfo[USTHumanoid.All.Count];

            void AddUST(USTHumanoid ust, string ustName)
            {
                USTInfos[ust.ID] = new USTInfo(ustName);
            }

            AddUST(USTHumanoid.None, "UST_None");
            AddUST(USTHumanoid.Neutral, "UST_None");

            AddUST(USTHumanoid.Angry_Gesture, "UST_AngryGesture");
            AddUST(USTHumanoid.ApplauseQuick, "UST_Applause_Quick");
            AddUST(USTHumanoid.DismissingGesture, "UST_Dismissing_Gesture");
            AddUST(USTHumanoid.Greet, "UST_Greet");
            AddUST(USTHumanoid.HeadTilt_Stretch, "UST_HeadTilt_Stretch");
            AddUST(USTHumanoid.JumpScared, "UST_Jump_Scared");
            AddUST(USTHumanoid.Jumpscare_HoldHeart, "UST_JumpScare_HoldHeart");
            AddUST(USTHumanoid.Look, "UST_Look");
            AddUST(USTHumanoid.LookBehind, "UST_LookBehind");
            AddUST(USTHumanoid.RollShoulder, "UST_RollShoulder");
            AddUST(USTHumanoid.SaluteBriefLeft, "UST_SaluteBriefLeft");
            AddUST(USTHumanoid.SaluteBriefRight, "UST_SaluteBriefRight");
            AddUST(USTHumanoid.ScratchHead, "UST_Head_Scratch");
            AddUST(USTHumanoid.Sigh_Relieved, "UST_Surprised_CoveringMouth");
            AddUST(USTHumanoid.Surprised_CoveringMouth, "UST_Surprised_CoveringMouth");
            AddUST(USTHumanoid.SwingArms_Subtle, "UST_SwingArms_Subtle");
            AddUST(USTHumanoid.TwistTorso_Subtle, "UST_TwistTorso_Subtle");
            AddUST(USTHumanoid.VictoryGesture, "UST_Victory_Gesture");
            AddUST(USTHumanoid.WipeForehead, "UST_Wipe_Forehead");
            AddUST(USTHumanoid.SaluteBrief, "UST_Salute_Brief");
            AddUST(USTHumanoid.InspectCuriousL, "UST_Inspect_Curious_L");
            AddUST(USTHumanoid.InspectCuriousR, "UST_Inspect_Curious_R");

            // ---------------------------------------
            // Write our results to sharedSettings
            sharedSettings.NRS = nRS;
            sharedSettings.RSInfos = RSInfos;
            sharedSettings.RStoRSMatrix = RStoRSMatrix;
            sharedSettings.RSTransitionInfos = RSTransitionInfos;
            sharedSettings.STTransitionInfos = STTransitionInfos;
            sharedSettings.USTInfos = USTInfos;
        }

        private void DebugShowRSTransitionMatrix(RStoRSMatrixEntry[] _RStoRSMatrix, int pad = 2)
        {
            var names = new List<string>(RSHumanoid.All.Select(rs => rs.Name));
            //string[] names = Enum.GetNames(typeof(RS));
            int nRS = names.Count;
            int breadth = 0;
            char padChar = '_';

            for (int i = 0; i < nRS; i++)
                breadth = Math.Max(breadth, names[i].Length);

            breadth += pad;

            for (int i = 0; i < nRS; i++)
                names[i] = names[i].PadRight(breadth, padChar);

            // now display matrix
            int rowLength = (nRS + 1) * breadth + 2;
            string none = "_XXX".PadRight(breadth, padChar);
            StringBuilder builder = new("".PadRight(breadth, padChar), (nRS + 1) * rowLength);

            for (int i = 0; i < nRS; i++)
                builder.Append(names[i]);

            builder.Append("\n");

            for (int i = 0; i < nRS; i++)
            {
                builder.Append(names[i]);

                for (int j = 0; j < nRS; j++)
                {
                    int index = _RStoRSMatrix[nRS * i + j].Next;

                    if (index >= 0)
                        builder.Append(names[index]);
                    else
                        builder.Append(none);
                }

                builder.Append("\n");
            }

            Debug.Log(builder);
        }
    }
}
