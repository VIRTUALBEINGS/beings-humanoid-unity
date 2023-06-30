// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class STHumanoid : STHumanoidBase
    {
        public static new STHumanoid None = new(0, nameof(None), true);
        public static new STHumanoid Reconfigure = new((int)ISTEnums.Reconfigure, nameof(Reconfigure), true);
        public static new STHumanoid Neutral = new((int)ISTEnums.Neutral, nameof(Neutral), true);
        public static new STHumanoid Turn = new((int)ISTEnums.Turn, nameof(Turn), true);

        public static STHumanoid StepL = new(nameof(StepL));
        public static STHumanoid StepR = new(nameof(StepR));

        public static STHumanoid Defeated          = new(nameof(Defeated));
        public static STHumanoid Greet             = new(nameof(Greet));
        public static STHumanoid GreetActive       = new(nameof(GreetActive));
        public static STHumanoid Reacting          = new(nameof(Reacting));
        public static STHumanoid Angry_Gesture     = new(nameof(Angry_Gesture));
        public static STHumanoid Angry_CrossArms   = new(nameof(Angry_CrossArms));
        public static STHumanoid StretchArms       = new(nameof(StretchArms));
        public static STHumanoid InspectCurious    = new(nameof(InspectCurious));
        public static STHumanoid Scared            = new(nameof(Scared));
        public static STHumanoid ScaredLeftRight   = new(nameof(ScaredLeftRight));
        public static STHumanoid Laugh             = new(nameof(Laugh));
        public static STHumanoid Applause          = new(nameof(Applause));
        public static STHumanoid Disappointed      = new(nameof(Disappointed));
        public static STHumanoid Shrug             = new(nameof(Shrug));
        public static STHumanoid SwingArms_Subtle  = new(nameof(SwingArms_Subtle));
        public static STHumanoid HeadTilt_Stretch  = new(nameof(HeadTilt_Stretch));
        public static STHumanoid RollShoulder      = new(nameof(RollShoulder));
        public static STHumanoid SatisfiedJumpy    = new(nameof(SatisfiedJumpy));
        public static STHumanoid TwistTorso_Subtle = new(nameof(TwistTorso_Subtle));
        public static STHumanoid Sad_FootKick      = new(nameof(Sad_FootKick));
        public static STHumanoid HandsOnBack       = new(nameof(HandsOnBack));
        public static STHumanoid HandsOnBackExit   = new(nameof(HandsOnBackExit));
        public static STHumanoid StretchArmsL      = new(nameof(StretchArmsL));
        public static STHumanoid StretchArmsR      = new(nameof(StretchArmsR));
        public static STHumanoid InspectCuriousL   = new(nameof(InspectCuriousL));
        public static STHumanoid InspectCuriousR   = new(nameof(InspectCuriousR));
        public static STHumanoid ShakeFootL        = new(nameof(ShakeFootL));
        public static STHumanoid ShakeFootR        = new(nameof(ShakeFootR));
        public static STHumanoid Inspect_Arms_WideOpen = new(nameof(Inspect_Arms_WideOpen));
        public static STHumanoid BalanceAdjust     = new(nameof(BalanceAdjust));


        public static STHumanoid Speaking_Long_Overt_Agreeing_RH     = new(nameof(Speaking_Long_Overt_Agreeing_RH));
        public static STHumanoid Speaking_Long_Overt_Explaining_BH   = new(nameof(Speaking_Long_Overt_Explaining_BH));
        public static STHumanoid Speaking_Long_Overt_Left_BH         = new(nameof(Speaking_Long_Overt_Left_BH));
        public static STHumanoid Speaking_Long_Overt_Explaining_Slow = new(nameof(Speaking_Long_Overt_Explaining_Slow));

        public static STHumanoid Speaking_Med_Overt_Argument_BH   = new(nameof(Speaking_Med_Overt_Argument_BH));
        public static STHumanoid Speaking_Med_Overt_Explaining_BH = new(nameof(Speaking_Med_Overt_Explaining_BH));
        public static STHumanoid Speaking_Med_Overt_Sigh_RH       = new(nameof(Speaking_Med_Overt_Sigh_RH));
        public static STHumanoid Speaking_Med_Subtle_Argument_RH  = new(nameof(Speaking_Med_Subtle_Argument_RH));

        public static STHumanoid Speaking_Short_Overt_Agreeing_BH  = new(nameof(Speaking_Short_Overt_Agreeing_BH));
        public static STHumanoid Speaking_Short_Overt_High_BH      = new(nameof(Speaking_Short_Overt_High_BH));
        public static STHumanoid Speaking_Short_Overt_Agreeing_RH  = new(nameof(Speaking_Short_Overt_Agreeing_RH));
        public static STHumanoid Speaking_Short_Subtle_Agreeing_BH = new(nameof(Speaking_Short_Subtle_Agreeing_BH));
        public static STHumanoid Speaking_Short_Subtle_ArmsOpen_BH = new(nameof(Speaking_Short_Subtle_ArmsOpen_BH));
        public static STHumanoid Speaking_Short_Subtle_Brief_RH    = new(nameof(Speaking_Short_Subtle_Brief_RH));
        public static STHumanoid Speaking_Short_Subtle_Twist_RH    = new(nameof(Speaking_Short_Subtle_Twist_RH));

        // STC
        public static STHumanoid Idle             = new(nameof(Idle), true);
        public static STHumanoid HandsOnBackCycle = new(nameof(HandsOnBackCycle), true);


        private STHumanoid(string name, bool isSTC = false) : base(isSTC, name) { }
        private STHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
