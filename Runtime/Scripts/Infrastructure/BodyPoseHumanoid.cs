// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class BodyPoseHumanoid : BodyPoseHumanoidBase
    {
        public static new BodyPoseHumanoid None { get; private set; } =
            new((int)IBodyAttitudeEnums.None, nameof(None), true);

        private BodyPoseHumanoid(string name) : base(name)
        {
        }

        private BodyPoseHumanoid(int id, string name, bool setNone) : base(id, name, setNone)
        {
        }
    }
}
