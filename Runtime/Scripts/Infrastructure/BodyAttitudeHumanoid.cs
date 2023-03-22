// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class BodyAttitudeHumanoid : BodyAttitudeHumanoidBase
    {
        // ------------------------
        // Body attitudes
        //

        public static new BodyAttitudeHumanoid None = new(
            (int)IBodyAttitudeEnums.None,
            nameof(None),
            Animator.StringToHash("BA_None"),
            true
        );

        public static BodyAttitudeHumanoid Idle { get; } = new(nameof(Idle), 1, Animator.StringToHash("BA_Idle"));
        public static BodyAttitudeHumanoid Lean { get; } = new(nameof(Lean), 1, Animator.StringToHash("BA_Lean"));

        // -----------------------
        //
        //

        public static new BodyAttitudeHumanoid FromID(int id) => (BodyAttitudeHumanoid)BodyAttitudeHumanoidBase.FromID(id);
        public static new BodyAttitudeHumanoid FromName(string name) =>
            (BodyAttitudeHumanoid)BodyAttitudeHumanoidBase.FromName(name);

        // --------------------------
        // Private stuff
        //

        private BodyAttitudeHumanoid(string name, int wideness, int hash) : base(name, wideness, hash)
        {
        }

        private BodyAttitudeHumanoid(int id, string name, int hash, bool setNone) : base(id, name, hash, setNone)
        {
        }
    }
}
