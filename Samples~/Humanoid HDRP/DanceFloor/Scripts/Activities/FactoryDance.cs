// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid.Samples.DanceSample
{
    [CreateAssetMenu(
        fileName = "Humanoid Root Activity - Dance - New.asset",
        menuName = "VIRTUAL BEINGS/Humanoid Root Activities/Dance",
        order = 1
    )]
    public class FactoryDance : RootActivityFactory
    {
        [SerializeField]
        private DanceSettings _settings;

        public override string MotiveName => "DANCE";

        public override void Generate(Mind mind, bool reinitMotive, out IRootActivity rootActivity)
        {
            Motive motive = new Motive(mind, this, true, 1 / 200f, 1 / 2000f);
            rootActivity = new Dance((HumanoidMind)mind, _settings);
        }
    }
}
