// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class FSTHumanoid : FSTHumanoidBase
    {
        public static new FSTHumanoid None          = new(0, nameof(None), true);
        public static new FSTHumanoid Reconfigure   = new((int)IFSTEnums.Reconfigure, nameof(Reconfigure), true);
        public static new FSTHumanoid Neutral       = new((int)IFSTEnums.Neutral, nameof(Neutral), true);

        public static FSTHumanoid RollingEyes       = new(nameof(RollingEyes));
        public static FSTHumanoid MouthPuckerSides  = new(nameof(MouthPuckerSides));

        private FSTHumanoid(string name) : base(name) { }
        private FSTHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
