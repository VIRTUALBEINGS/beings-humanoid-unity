// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEditor;
using VirtualBeings.Tech.BehaviorComposition;

namespace VirtualBeings.Beings.Humanoid
{
    public class USTHumanoid : USTHumanoidBase
    {
        public static new USTHumanoid None = new((int)IUSTEnums.None, nameof(None), true);

        public static USTHumanoid Neutral                = new(nameof(Neutral));

        public static USTHumanoid Angry_Gesture           = new(nameof(Angry_Gesture));
        public static USTHumanoid ApplauseQuick           = new(nameof(ApplauseQuick));
        public static USTHumanoid DismissingGesture       = new(nameof(DismissingGesture));
        public static USTHumanoid Greet                   = new(nameof(Greet));
        public static USTHumanoid HeadTilt_Stretch        = new(nameof(HeadTilt_Stretch));
        public static USTHumanoid JumpScared              = new(nameof(JumpScared));
        public static USTHumanoid Jumpscare_HoldHeart     = new(nameof(Jumpscare_HoldHeart));
        public static USTHumanoid Look                    = new(nameof(Look));
        public static USTHumanoid LookBehind              = new(nameof(LookBehind));
        public static USTHumanoid RollShoulder            = new(nameof(RollShoulder));
        public static USTHumanoid SaluteBriefLeft         = new(nameof(SaluteBriefLeft));
        public static USTHumanoid SaluteBriefRight        = new(nameof(SaluteBriefRight));
        public static USTHumanoid ScratchHead             = new(nameof(ScratchHead));
        public static USTHumanoid Sigh_Relieved           = new(nameof(Sigh_Relieved));
        public static USTHumanoid Surprised_CoveringMouth = new(nameof(Surprised_CoveringMouth));
        public static USTHumanoid SwingArms_Subtle        = new(nameof(SwingArms_Subtle));
        public static USTHumanoid TwistTorso_Subtle       = new(nameof(TwistTorso_Subtle));
        public static USTHumanoid VictoryGesture          = new(nameof(VictoryGesture));
        public static USTHumanoid WipeForehead            = new(nameof(WipeForehead));
        public static USTHumanoid SaluteBrief             = new(nameof(SaluteBrief));
        public static USTHumanoid InspectCuriousL         = new(nameof(InspectCuriousL));
        public static USTHumanoid InspectCuriousR         = new(nameof(InspectCuriousR));

        private USTHumanoid(string name) : base(name) { }
        private USTHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
