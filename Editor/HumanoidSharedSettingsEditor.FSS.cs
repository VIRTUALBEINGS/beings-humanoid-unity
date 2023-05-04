using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;

namespace VirtualBeings.Beings.Humanoid
{
    public partial class HumanoidSharedSettingsEditor
    {
        private void InitFSTransitionMatrix(int nFS, FSInfo[] FSInfos, FStoFSMatrixEntry[] FStoFSMatrix)
        {
        }

        private void OnInspectorGUI_InitActuatorData_FSS(HumanoidSharedClientSettings sharedSettings)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // ---------------------------------------
            int nFS = FSHumanoid.All.Count;

            int FS_NEUTRAL       = FSHumanoid.None.ID;
            int FS_SHY           = FSHumanoid.SHY.ID;
            int FS_SATISFIED     = FSHumanoid.SATISFIED.ID;
            int FS_INTERESTED    = FSHumanoid.INTERESTED.ID;
            int FS_AVERSE        = FSHumanoid.AVERSE.ID;
            int FS_SAD           = FSHumanoid.SAD.ID;
            int FS_ASSERTIVE     = FSHumanoid.ASSERTIVE.ID;
            int FS_Confused      = FSHumanoid.Confused.ID;
            int FS_PuffedCheeks  = FSHumanoid.PuffedCheeks.ID;
            int FS_Grimace       = FSHumanoid.Grimace.ID;
            int FS_EyesSquinted  = FSHumanoid.EyesSquinted.ID;
            int FS_EyebrowsRaise = FSHumanoid.EyebrowsRaise.ID;
            int FS_SeriousLook   = FSHumanoid.SeriousLook.ID;
            int FS_SmileSubtle   = FSHumanoid.SmileSubtle.ID;
            int FS_SmileWide     = FSHumanoid.SmileWide.ID;
            int FS_Annoyed       = FSHumanoid.Annoyed.ID;
            int FS_Bored         = FSHumanoid.Bored.ID;
            int FS_Curious       = FSHumanoid.Curious.ID;

            const string STATE_NEUTRAL        = "Face_Neutral";
            const string STATE_SHY            = "Face_Shy";
            const string STATE_SATISFIED      = "Face_Satisfied";
            const string STATE_INTERESTED     = "Face_Interested";
            const string STATE_AVERSE         = "Face_Averse";
            const string STATE_SAD            = "Face_Sad";
            const string STATE_ASSERTIVE      = "Face_Assertive";
            const string STATE_CONFUSED       = "Face_Confused";
            const string STATE_PUFFED_CHEEKS  = "Face_PuffedCheeks";
            const string STATE_GRIMACE        = "Face_Grimace";
            const string STATE_EYES_SQUINTED  = "Face_EyesSquinted";
            const string STATE_EYEBROWS_RAISE = "Face_EyebrowsRaise";
            const string STATE_SERIOUS_LOOK   = "Face_SeriousLook";
            const string STATE_SMILE_SUBTLE   = "Face_SmileSubtle";
            const string STATE_SMILE_WIDE     = "Face_SmileWide";
            const string STATE_ANNOYED        = "Face_Annoyed";
            const string STATE_BORED          = "Face_Bored";
            const string STATE_CURIOUS        = "Face_Curious";

            // ---------------------------------------
            FSInfo[] FSInfos = new FSInfo[nFS];
            List<FSTTransitionInfo> FSTTransitionInfosL = new();

            int AddS(FSTTransitionInfo transitionInfo)
            {
                FSTTransitionInfosL.Add(transitionInfo);
                return FSTTransitionInfosL.Count - 1;
            }

            // NB: unnecessary to add FSInfo for FSHumanoid.None (why? because unity deserialization
            // instantiates null fields of custom classes and because FSInfo implements ISerializationCallbackReceiver)
            // OBS: each of the Animator states referenced here must *exist* and have the *tag* 'TransitionBehavior' (checked automatically below)

            void AddBasicFS(int fsID, string fsName, bool supportsFreeEntryAndExit = true)
            {
                FSInfos[fsID] = new FSInfo(
                    Animator.StringToHash(fsName),
                    fsName,
                    supportsFreeEntryAndExit,
                    new FSInfo.FST_and_int[] { }
                );
            }

            FSInfos[FS_NEUTRAL] = new FSInfo(
                Animator.StringToHash(STATE_NEUTRAL),
                STATE_NEUTRAL,
                true,
                new FSInfo.FST_and_int[]
                {
                    new(FSTHumanoid.RollingEyes.ID, AddS(new FSTTransitionInfo("FST_RollingEyes"))),
                    new(FSTHumanoid.MouthPuckerSides.ID, AddS(new FSTTransitionInfo("FST_MouthPuckerSides"))),
                    new(FSTHumanoid.Grimace.ID, AddS(new FSTTransitionInfo("FST_Grimace"))),
                    new(FSTHumanoid.PuffedCheeksBlow.ID, AddS(new FSTTransitionInfo("FST_PuffedCheeksBlow"))),
                    new(FSTHumanoid.Rage.ID, AddS(new FSTTransitionInfo("FST_Rage"))),
                    new(FSTHumanoid.SuspiciousLook.ID, AddS(new FSTTransitionInfo("FST_SuspiciousLook"))),
                    new(FSTHumanoid.Wink.ID, AddS(new FSTTransitionInfo("FST_Wink"))),
                    new(FSTHumanoid.Laugh.ID, AddS(new FSTTransitionInfo("FST_Laugh"))),
                }
            );

            AddBasicFS(FS_SHY, STATE_SHY);
            AddBasicFS(FS_SATISFIED, STATE_SATISFIED);
            AddBasicFS(FS_INTERESTED, STATE_INTERESTED);
            AddBasicFS(FS_AVERSE, STATE_AVERSE);
            AddBasicFS(FS_SAD, STATE_SAD);
            AddBasicFS(FS_ASSERTIVE, STATE_ASSERTIVE);
            AddBasicFS(FS_Confused, STATE_CONFUSED);
            AddBasicFS(FS_PuffedCheeks, STATE_PUFFED_CHEEKS);
            AddBasicFS(FS_Grimace, STATE_GRIMACE);
            AddBasicFS(FS_EyesSquinted, STATE_EYES_SQUINTED);
            AddBasicFS(FS_EyebrowsRaise, STATE_EYEBROWS_RAISE);
            AddBasicFS(FS_SeriousLook, STATE_SERIOUS_LOOK);
            AddBasicFS(FS_SmileSubtle, STATE_SMILE_SUBTLE);
            AddBasicFS(FS_SmileWide, STATE_SMILE_WIDE);
            AddBasicFS(FS_Annoyed, STATE_ANNOYED);
            AddBasicFS(FS_Bored, STATE_BORED);
            AddBasicFS(FS_Curious, STATE_CURIOUS);

            // copy List(T) FSTTransitionInfosL into array
            FSTTransitionInfo[] FSTTransitionInfos = new FSTTransitionInfo[FSTTransitionInfosL.Count];
            for (int i = 0; i < FSTTransitionInfosL.Count; i++)
            {
                FSTTransitionInfos[i] = FSTTransitionInfosL[i];
            }

            // ---------------------------------------

            FStoFSMatrixEntry[] FStoFSMatrix = new FStoFSMatrixEntry[nFS * nFS];
            List<FSTransitionInfo> FSTransitionInfosL = new();

             int AddP(FSTransitionInfo transitionInfo)
            {
                FSTransitionInfosL.Add(transitionInfo);
                return FSTransitionInfosL.Count - 1;
            }

            // init whole matrix with 'unavailable' transitions
            for (int i = 0; i < FStoFSMatrix.Length; i++)
            {
                FStoFSMatrix[i] = new FStoFSMatrixEntry(FSHumanoid.None.ID, -1);
            }

            // NB: none of the entries below initialize FStoFSMatrixEntry.Variations because it can't be serialized;
            // instead Variations is reconstructed from the linked list of 'FSTransitionInfo.NextExpressiveVariation's
            // in BirdBeing

            // ---------------------------------------

            //FStoFSMatrix[FS_SATISFIED * nFS + FS_SAD] = new FStoFSMatrixEntry(FSHumanoid.SAD.ID,
            //    AddP(new FSTransitionInfo(Animator.StringToHash("Face_Satisfied_to_Sad"), FTTBird.Satisfied_to_Sad_Disappointed.ID)));

            // Neutral to Averse
            // FStoFSMatrix[FS_NEUTRAL * nFS + FS_AVERSE] = new FStoFSMatrixEntry(FSHumanoid.AVERSE.ID,
                // AddP(new FSTransitionInfo(Animator.StringToHash("Face_Neutral_To_Averse"), FTTHumanoid.Default.ID)));

            // ---------------------------------------
            // TODO in the future, when there's states that dont support crossfade from all other crossfadeable states
            // (e.g. with tongue out), we need to add code to this method to ensure that such states can reach all other
            // states via *single* transition states. this requires they always have indices != -1 in 'their'
            // FStoFSMatrixEntrys, and conversely all other states point to them via indices that are also != -1.
            //
            // Note that *all other* indices *can* be -1. In Actuator.FacialStateControl.cs, this means that a direct
            // crossfade will be done

            int AddDefaultTransition(int FS) => AddP(new FSTransitionInfo(FSInfos[FS].StateHash));

            FStoFSMatrixEntry DefaultTransition(int FS) => new(FS, AddDefaultTransition(FS));

            FStoFSMatrixEntry NamedTransition(int FS, string FOT) =>
                new(FS, AddP(new FSTransitionInfo(Animator.StringToHash(FOT))));

            void InsertDefaultFOT(int to, int from)
            {
                FStoFSMatrix[from * nFS + to] = DefaultTransition(to);
            }

            void InsertNamedFOT(int to, int from, string transitionName)
            {
                FStoFSMatrix[from * nFS + to] = NamedTransition(to, transitionName);
            }

            InitFSTransitionMatrix(nFS, FSInfos, FStoFSMatrix);

            InsertNamedFOT(FS_NEUTRAL, FS_SATISFIED, "Face_Satisfied_to_Neutral");
            InsertNamedFOT(FS_NEUTRAL, FS_AVERSE, "Face_Averse_to_Neutral");
            InsertNamedFOT(FS_NEUTRAL, FS_SAD, "Face_Sad_to_Neutral");
            InsertNamedFOT(FS_NEUTRAL, FS_ASSERTIVE, "Face_Assertive_to_Neutral");

            InsertNamedFOT(FS_SATISFIED, FS_NEUTRAL, "Face_Neutral_to_Satisfied");
            InsertNamedFOT(FS_AVERSE, FS_NEUTRAL, "Face_Neutral_to_Averse");
            InsertNamedFOT(FS_SAD, FS_NEUTRAL, "Face_Neutral_to_Sad");
            InsertNamedFOT(FS_ASSERTIVE, FS_NEUTRAL, "Face_Neutral_to_Assertive");

            // ---------------------------------------

            // copy List(T) FSTransitionInfosL into array
            FSTransitionInfo[] FSTransitionInfos = new FSTransitionInfo[FSTransitionInfosL.Count];
            for (int i = 0; i < FSTransitionInfosL.Count; i++)
            {
                FSTransitionInfos[i] = FSTransitionInfosL[i];
            }

            // check that all referenced FOTs and FSTs exist and are correctly tagged

            string[] animatorControllerGuid = AssetDatabase.FindAssets(sharedSettings.AnimatorController.name);
            AnimatorController controller = (AnimatorController)AssetDatabase.LoadAssetAtPath(
                AssetDatabase.GUIDToAssetPath(animatorControllerGuid[0]),
                typeof(AnimatorController)
            );
            ChildAnimatorState[] states = controller.layers[sharedSettings.LayerIndexEyes].stateMachine
                                                    .states;

            // go through FSTransitionInfosL and check if all its state hashes point to existing states
            foreach (FSTransitionInfo info in FSTransitionInfosL)
            {
                bool found = states.Select(t => t.state).Any(state => state.nameHash == info.Hash);

                // NB: if this returns an error, note down the Hash and set a conditional breakpoint in
                // the constructor of FSTransitionInfo in BirdAnimStates.cs in Rascal
                if (!found)
                {
                    Debug.LogError($"Error checking OT #{info.Hash}: not found");
                }
            }

            // go through FSTTransitionInfosL and check if all its state hashes point to existing states and are correctly tagged
            foreach (FSTTransitionInfo info in FSTTransitionInfosL)
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
                    Debug.Log($"Checking ST #{info.Name}: not found");
                }
                else if (!hasTransitionTag)
                {
                    Debug.Log($"Checking ST #{info.Name}: transition tag not present");
                }
            }

            // ---------------------------------------
            // Write our results to sharedSettings

            sharedSettings.NFS = nFS;
            sharedSettings.FSInfos = FSInfos;
            sharedSettings.FStoFSMatrix = FStoFSMatrix;
            sharedSettings.FSTransitionInfos = FSTransitionInfos;
            sharedSettings.FSTTransitionInfos = FSTTransitionInfos;
        }
    }
}
