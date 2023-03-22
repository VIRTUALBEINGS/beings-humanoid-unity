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

        public static USTHumanoid Neutral               = new(nameof(Neutral));
        public static USTHumanoid SaluteBriefLeft       = new(nameof(SaluteBriefLeft));
        public static USTHumanoid SaluteBriefRight      = new(nameof(SaluteBriefRight));
        public static USTHumanoid ApplauseQuick         = new(nameof(ApplauseQuick));
        public static USTHumanoid DismissingGesture     = new(nameof(DismissingGesture));
        public static USTHumanoid VictoryGesture        = new(nameof(VictoryGesture));
        public static USTHumanoid JumpScared            = new(nameof(JumpScared));
        public static USTHumanoid ScratchHead           = new(nameof(ScratchHead));
        public static USTHumanoid WipeForehead          = new(nameof(WipeForehead));

        private USTHumanoid(string name) : base(name) { }
        private USTHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
