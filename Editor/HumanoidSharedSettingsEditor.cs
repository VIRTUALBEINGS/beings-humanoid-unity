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
            int RS_Walk               = RSHumanoid.Walk.ID;
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
                new int[] { BodyAttitudeHumanoid.Idle.ID, BodyAttitudeHumanoid.Lean.ID },
                new RSInfo.ST_and_int[]
                {
                    new(STHumanoid.Neutral.ID, AddS("ST_Stand_Neutral", maxCrossFadeDurationOfTransitionParameters: 0.1f)),

                    new(STHumanoid.Turn.ID, AddS("ST_Stand_Turn", maxCrossFadeDurationOfTransitionParameters: 0f)),
                    new(STHumanoid.StepL.ID, AddS("ST_Stand_StepL", maxCrossFadeDurationOfTransitionParameters: 0f)),
                    new(STHumanoid.StepR.ID, AddS("ST_Stand_StepR", maxCrossFadeDurationOfTransitionParameters: 0f)),

                    new(STHumanoid.Defeated.ID, AddS("ST_Stand_Defeated", STCrossfadeTime)),
                    new(STHumanoid.GreetActive.ID, AddS("ST_Stand_Greet_Active", STCrossfadeTime)),
                    new(STHumanoid.Greet.ID, AddS("ST_Stand_Greet", STCrossfadeTime)),
                    new(STHumanoid.Reacting.ID, AddS("ST_Stand_Reacting", STCrossfadeTime)),
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
                }
            );

            RSInfos[RS_Walk] = new RSInfo(
                Animator.StringToHash("Root_Walk"),
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

            float RSInterruptionTime = sharedSettings.CrossFadeTimeForRSInterruption;

            for (int i = 0; i < RStoRSMatrix.Length; i++)
            {
                RStoRSMatrix[i] = new RStoRSMatrixEntry(RSHumanoid.None.ID, -1);
            }

            // NB: none of the entries below initialize RStoRSMatrixEntry.Variations because it can't be serialized;
            // instead Variations is reconstructed from the linked list of 'RSTransitionInfo.NextExpressiveVariation's in BirdBeing

            int AddDefaultTransition(int RS) =>
                AddP(
                    new RSTransitionInfo(RSInfos[RS].StateHash, TransitionTypeHumanoid.Default.ID, RSInterruptionTime)
                );

            RStoRSMatrixEntry DefaultTransition(int RS) => new(RS, AddDefaultTransition(RS));

            RStoRSMatrixEntry NamedTransition(int RS, string OT)
            {
                Debug.Log($"{OT}: {Animator.StringToHash(OT)}");
                return new(
                    RS,
                    AddP(
                        new RSTransitionInfo(
                            Animator.StringToHash(OT),
                            TransitionTypeHumanoid.Default.ID,
                            RSInterruptionTime
                        )
                    )
                );
            }

            void InsertDefaultOT(int to, int from)
            {
                RStoRSMatrix[from * nRS + to] = DefaultTransition(to);
            }

            void InsertNamedOT(int to, int from, string transitionName)
            {
                RStoRSMatrix[from * nRS + to] = NamedTransition(to, transitionName);
            }

            // ---------------------------------------
            // -> Stand
            InsertDefaultOT(RS_Stand, RS_Stand);
            InsertDefaultOT(RS_Stand, RS_Walk);
            InsertNamedOT(RS_Stand, RS_Dance_LowEnergy, "Dance_LowEnergy_to_Stand");
            InsertNamedOT(RS_Stand, RS_Dance_Robot, "Dance_Robot_to_Stand");
            InsertNamedOT(RS_Stand, RS_Dance_Wave, "Dance_Wave_to_Stand");
            InsertNamedOT(RS_Stand, RS_Dance_Samba, "Dance_Samba_to_Stand");
            InsertNamedOT(RS_Stand, RS_Dance_GangnamStyle, "Dance_GangNamStyle_to_Stand");

            // ---------------------------------------
            // -> Walk
            InsertDefaultOT(RS_Walk, RS_Walk);
            RStoRSMatrix[RS_Stand * nRS + RS_Walk] = new(
                RS_Walk,
                AddP(
                    new RSTransitionInfo(
                        Animator.StringToHash("Stand_to_WalkL"),
                        TransitionTypeHumanoid.DefaultLeft.ID,
                        nextExpressiveVariation: AddP(
                            new RSTransitionInfo(
                                Animator.StringToHash("Stand_to_WalkR"),
                                TransitionTypeHumanoid.DefaultRight.ID,
                                nextExpressiveVariation: AddP(
                                    new RSTransitionInfo(
                                        Animator.StringToHash("Stand_to_Walk_BackwardsL"),
                                        TransitionTypeHumanoid.DefaultBackwardLeft.ID,
                                        nextExpressiveVariation: AddP(
                                        new RSTransitionInfo(
                                            Animator.StringToHash("Stand_to_Walk_BackwardsR"),
                                            TransitionTypeHumanoid.DefaultBackwardRight.ID)
                                            // nextExpressiveVariation: AddP(new RSTransitionInfo(Animator.StringToHash("Stand_to_Walk_AtAngleL"), TransitionTypeHumanoid.AtAngleLeft.ID,
                                            // nextExpressiveVariation: AddP(new RSTransitionInfo(Animator.StringToHash("Stand_to_Walk_AtAngleR"), TransitionTypeHumanoid.AtAngleRight.ID)))))))));
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );

            InsertNamedOT(RS_Dance_LowEnergy, RS_Stand, "Stand_to_Dance_LowEnergy");
            InsertNamedOT(RS_Dance_Robot, RS_Stand, "Stand_to_Dance_Robot");
            InsertNamedOT(RS_Dance_Wave, RS_Stand, "Stand_to_Dance_Wave");
            InsertNamedOT(RS_Dance_Samba, RS_Stand, "Stand_to_Dance_Samba");
            InsertNamedOT(RS_Dance_GangnamStyle, RS_Stand, "Stand_to_Dance_GangNamStyle");

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

            int UST_None              = USTHumanoid.None.ID;
            int UST_Neutral           = USTHumanoid.Neutral.ID;
            int UST_SaluteBriefL      = USTHumanoid.SaluteBriefLeft.ID;
            int UST_SaluteBriefR      = USTHumanoid.SaluteBriefRight.ID;
            int UST_ApplauseQuick     = USTHumanoid.ApplauseQuick.ID;
            int UST_DismissingGesture = USTHumanoid.DismissingGesture.ID;
            int UST_VictoryGesture    = USTHumanoid.VictoryGesture.ID;
            int UST_JumpScared        = USTHumanoid.JumpScared.ID;
            int UST_ScratchHead       = USTHumanoid.ScratchHead.ID;
            int UST_WipeForehead      = USTHumanoid.WipeForehead.ID;
            int UST_SwingArms         = USTHumanoid.SwingArms_Subtle.ID;
            int UST_HeadTiltStretch   = USTHumanoid.HeadTilt_Stretch.ID;
            int UST_RollShoulder      = USTHumanoid.RollShoulder.ID;
            int UST_TwistTorso_Subtle = USTHumanoid.TwistTorso_Subtle.ID;

            USTInfo[] USTInfos              = new USTInfo[USTHumanoid.All.Count];
            USTInfos[UST_None]              = new USTInfo("UST_None");
            USTInfos[UST_Neutral]           = new USTInfo("UST_None");
            USTInfos[UST_SaluteBriefL]      = new USTInfo("UST_SaluteBriefLeft");
            USTInfos[UST_SaluteBriefR]      = new USTInfo("UST_SaluteBriefRight");
            USTInfos[UST_ApplauseQuick]     = new USTInfo("UST_Applause_Quick");
            USTInfos[UST_DismissingGesture] = new USTInfo("UST_Dismissing_Gesture");
            USTInfos[UST_VictoryGesture]    = new USTInfo("UST_Victory_Gesture");
            USTInfos[UST_JumpScared]        = new USTInfo("UST_Jump_Scared");
            USTInfos[UST_ScratchHead]       = new USTInfo("UST_Head_Scratch");
            USTInfos[UST_WipeForehead]      = new USTInfo("UST_Wipe_Forehead");
            USTInfos[UST_SwingArms]         = new USTInfo("UST_SwingArms_Subtle");
            USTInfos[UST_HeadTiltStretch]   = new USTInfo("UST_HeadTilt_Stretch");
            USTInfos[UST_RollShoulder]      = new USTInfo("UST_RollShoulder");
            USTInfos[UST_TwistTorso_Subtle] = new USTInfo("UST_TwistTorso_Subtle");

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
