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
        public static BodyAttitudeHumanoid Angry { get; } = new (nameof(Angry), 1, Animator.StringToHash("BA_Angry"));
        public static BodyAttitudeHumanoid Confused { get; } = new (nameof(Confused), 1, Animator.StringToHash("BA_Confused"));
        public static BodyAttitudeHumanoid Contrapposto { get; } = new (nameof(Contrapposto), 1, Animator.StringToHash("BA_Contrapposto"));
        public static BodyAttitudeHumanoid Defeated { get; } = new (nameof(Defeated), 1, Animator.StringToHash("BA_Defeated"));
        public static BodyAttitudeHumanoid Disgusted { get; } = new (nameof(Disgusted), 1, Animator.StringToHash("BA_Disgusted"));
        public static BodyAttitudeHumanoid Judging { get; } = new (nameof(Judging), 1, Animator.StringToHash("BA_Judging"));
        public static BodyAttitudeHumanoid LegJoined { get; } = new (nameof(LegJoined), 1, Animator.StringToHash("BA_LegJoined"));
        public static BodyAttitudeHumanoid LegSpread { get; } = new (nameof(LegSpread), 1, Animator.StringToHash("BA_LegSpread"));
        public static BodyAttitudeHumanoid Relaxed { get; } = new (nameof(Relaxed), 1, Animator.StringToHash("BA_Relaxed"));
        public static BodyAttitudeHumanoid Scared { get; } = new (nameof(Scared), 1, Animator.StringToHash("BA_Scared"));
        public static BodyAttitudeHumanoid HandsBehindBack { get; } = new(nameof(HandsBehindBack), 1, Animator.StringToHash("BA_HandsBehindBack"));
        public static BodyAttitudeHumanoid HandsOnHips { get; } = new(nameof(HandsOnHips), 1, Animator.StringToHash("BA_HandsOnHips"));

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
