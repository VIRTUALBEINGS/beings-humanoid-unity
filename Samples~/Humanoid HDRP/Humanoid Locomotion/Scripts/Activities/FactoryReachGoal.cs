// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid.Samples.LocomotionSample
{
    [CreateAssetMenu(
        fileName = "Humanoid Root Activity - Reach Goal - New.asset",
        menuName = "VIRTUAL BEINGS/Humanoid Root Activities/Reach Goal",
        order = 1
    )]
    public class FactoryReachGoal : RootActivityFactory
    {
        [SerializeField]
        private ReachGoalSettings _settings;

        public override string MotiveName => "REACH GOAL";

        public override void Generate(Mind mind, bool reinitMotive, out IRootActivity rootActivity, out Motive motive)
        {
            motive = new Motive(mind, this, true, 1 / 200f, 1 / 2000f);
            ((HumanoidSelf)mind.Self).AddMotive(motive);
            rootActivity = new ReachGoal((HumanoidMind)mind, _settings);
        }
    }
}
