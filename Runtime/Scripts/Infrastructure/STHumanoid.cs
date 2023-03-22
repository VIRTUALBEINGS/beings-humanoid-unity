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

        public static STHumanoid StepL = new(false, nameof(StepL));
        public static STHumanoid StepR = new(false, nameof(StepR));

        public static STHumanoid Defeated           = new(false, nameof(Defeated));
        public static STHumanoid Greet              = new(false, nameof(Greet));
        public static STHumanoid GreetActive        = new(false, nameof(GreetActive));
        public static STHumanoid Reacting           = new(false, nameof(Reacting));
        public static STHumanoid Angry_CrossArms    = new(false, nameof(Angry_CrossArms));
        public static STHumanoid StretchArms        = new(false, nameof(StretchArms));
        public static STHumanoid InspectCurious     = new(false, nameof(InspectCurious));
        public static STHumanoid Scared             = new(false, nameof(Scared));
        public static STHumanoid ScaredLeftRight    = new(false, nameof(ScaredLeftRight));
        public static STHumanoid Laugh              = new(false, nameof(Laugh));
        public static STHumanoid Applause           = new(false, nameof(Applause));
        public static STHumanoid Disappointed       = new(false, nameof(Disappointed));
        public static STHumanoid Shrug              = new(false, nameof(Shrug));

        private STHumanoid(bool isSTC, string name) : base(isSTC, name) { }
        private STHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
