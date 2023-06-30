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
        public static new FSTHumanoid None        = new(0, nameof(None), true);
        public static new FSTHumanoid Reconfigure = new((int)IFSTEnums.Reconfigure, nameof(Reconfigure), true);
        public static new FSTHumanoid Neutral     = new((int)IFSTEnums.Neutral, nameof(Neutral), true);

        public static FSTHumanoid GotIdea                   = new(nameof(GotIdea));
        public static FSTHumanoid Grimace                   = new(nameof(Grimace));
        public static FSTHumanoid Nod                       = new(nameof(Nod));
        public static FSTHumanoid HeadShake                 = new(nameof(HeadShake));
        public static FSTHumanoid Laugh                     = new(nameof(Laugh));
        public static FSTHumanoid LaughFeminine             = new(nameof(LaughFeminine));
        public static FSTHumanoid LaughFeminine_Embarrassed = new(nameof(LaughFeminine_Embarrassed));
        public static FSTHumanoid MouthPuckerSides          = new(nameof(MouthPuckerSides));
        public static FSTHumanoid PuffedCheeksBlow          = new(nameof(PuffedCheeksBlow));
        public static FSTHumanoid Rage                      = new(nameof(Rage));
        public static FSTHumanoid RollingEyes               = new(nameof(RollingEyes));
        public static FSTHumanoid Shock                     = new(nameof(Shock));
        public static FSTHumanoid SuspiciousLook            = new(nameof(SuspiciousLook));
        public static FSTHumanoid Wink                      = new(nameof(Wink));
        public static FSTHumanoid SaluteHead               = new(nameof(SaluteHead));

        private FSTHumanoid(string name) : base(name) { }
        private FSTHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
