// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid.Sample.QuickStart
{
    [CreateAssetMenu(
        fileName = "Humanoid Root Activity - Explore Sample - New.asset",
        menuName = "VIRTUAL BEINGS/Humanoid Root Activities/Explore Sample",
        order = 1
    )]
    public class FactoryExplore : RootActivityFactory
    {
        [SerializeField]
        private ExploreSettings _settings;

        public override string MotiveName => "EXPLORE SAMPLE";

        public override void Generate(Mind mind, bool reinitMotive, out IRootActivity rootActivity)
        {
            Motive motive = new Motive(mind, this, true, 1 / 200f, 1 / 2000f);
            rootActivity = new Explore((HumanoidMind)mind, motive, _settings);
        }
    }
}
