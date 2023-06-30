// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class RSHumanoid : RSHumanoidBase
    {
        public static new RSHumanoid None = new((int)IRSEnums.None, nameof(None), true);
        public static new RSHumanoid Stand = new((int)IRSEnums.Stand, nameof(Stand), true);

        public static RSHumanoid Walk = new(nameof(Walk), 0, isMoving: true);

        public static RSHumanoid Dance_LowEnergy    = new(nameof(Dance_LowEnergy), 0, isStanding: true);
        public static RSHumanoid Dance_Robot        = new(nameof(Dance_Robot), 0, isStanding: true);
        public static RSHumanoid Dance_Wave         = new(nameof(Dance_Wave), 0, isStanding: true);
        public static RSHumanoid Dance_Samba        = new(nameof(Dance_Samba), 0, isStanding: true);
        public static RSHumanoid Dance_GangnamStyle = new(nameof(Dance_GangnamStyle), 0, isStanding: true);

        public static RSHumanoid Sit  = new(nameof(Sit), 0, isSitting: true);

        public static RSHumanoid Jog = new(nameof(Jog), 0, isMoving: true);


        private RSHumanoid(
            string name,
            int side,
            bool isLying = false,
            bool isSitting = false,
            bool isStanding = false,
            bool isMoving = false,
            bool isFlying = false,
            StateUpness upness = StateUpness.Up
        )
            : base(name, side, isLying, isSitting, isStanding, isMoving, isFlying, upness)
        {
        }

        private RSHumanoid(int id, string name, bool setSpecial)
            : base(id, name, setSpecial)
        {
        }
    }
}
