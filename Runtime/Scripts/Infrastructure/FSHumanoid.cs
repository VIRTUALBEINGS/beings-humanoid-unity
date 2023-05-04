// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class FSHumanoid : FSHumanoidBase
    {
        public static new FSHumanoid None = new(0, nameof(None), 0.50f, false, true);

        public static new FSHumanoid SHY        = new((int)IFSEnums.SHY, nameof(SHY), 0.33f, false, true);
        public static new FSHumanoid SATISFIED  = new((int)IFSEnums.SATISFIED, nameof(SATISFIED), 0.55f, false, true);
        public static new FSHumanoid INTERESTED = new((int)IFSEnums.INTERESTED, nameof(INTERESTED), 0.65f, false, true);
        public static new FSHumanoid AVERSE     = new((int)IFSEnums.AVERSE, nameof(AVERSE), 0.60f, false, true);
        public static new FSHumanoid SAD        = new((int)IFSEnums.SAD, nameof(SAD), 0.20f, false, true);
        public static new FSHumanoid ASSERTIVE  = new((int)IFSEnums.ASSERTIVE, nameof(ASSERTIVE), 0.75f, false, true);

        public static FSHumanoid Confused      = new(nameof(Confused), 0.75f, false);
        public static FSHumanoid PuffedCheeks  = new(nameof(PuffedCheeks), 0.5f, false);
        public static FSHumanoid Grimace       = new(nameof(Grimace), 0.5f, false);
        public static FSHumanoid EyesSquinted  = new(nameof(EyesSquinted), 0.5f, false);
        public static FSHumanoid EyebrowsRaise = new(nameof(EyebrowsRaise), 0.5f, false);
        public static FSHumanoid SeriousLook   = new(nameof(SeriousLook), 0.5f, false);
        public static FSHumanoid SmileSubtle   = new(nameof(SmileSubtle), 0.5f, false);
        public static FSHumanoid SmileWide     = new(nameof(SmileWide), 0.5f, false);
        public static FSHumanoid Annoyed       = new(nameof(Annoyed), 0.5f, false);
        public static FSHumanoid Bored         = new(nameof(Bored), 0.5f, false);
        public static FSHumanoid Curious       = new(nameof(Curious), 0.5f, false);

        private FSHumanoid(string name, float vivacity, bool eyesClosed) : base(name, vivacity, eyesClosed)
        {
        }

        private FSHumanoid(int id, string name, float vivacity, bool eyesClosed, bool setSpecial)
            : base(id, name, vivacity, eyesClosed, setSpecial)
        {
        }
    }
}
