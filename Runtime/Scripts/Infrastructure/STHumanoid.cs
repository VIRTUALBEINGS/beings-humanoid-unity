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

        private STHumanoid(string name, bool isSTC = false) : base(isSTC, name) { }
        private STHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
