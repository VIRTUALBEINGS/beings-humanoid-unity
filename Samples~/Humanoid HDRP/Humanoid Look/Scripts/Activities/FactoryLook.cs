// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid.Samples.LookSample
{

    [CreateAssetMenu(
        fileName = "Humanoid Root Activity - Look Sample - New.asset",
        menuName = "VIRTUAL BEINGS/Humanoid Root Activities/Look Sample",
        order = 1
    )]

    [Serializable]
    public class FactoryLook : RootActivityFactory
    {
        [SerializeField]
        private LookSettings _settings;

        public override string MotiveName => "LOOK SAMPLE";

        public override void Generate(Mind mind, bool reinitMotive, out IRootActivity rootActivity)
        {
            Motive motive = new Motive(mind, this, true, 1 / 200f, 1 / 2000f);
            rootActivity = new Look((HumanoidMind)mind, _settings);
        }
    }
}
