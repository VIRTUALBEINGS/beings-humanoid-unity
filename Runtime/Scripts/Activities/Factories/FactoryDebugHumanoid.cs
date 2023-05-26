// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;

namespace VirtualBeings.Tech.Beings.Humanoid
{
    [CreateAssetMenu(
        fileName = "Humanoid Root Activity - Debug Animation - New.asset",
        menuName = "VIRTUAL BEINGS/Humanoid Root Activities/Debug Animation",
        order = 1
    )]
    public class FactoryDebugHumanoid : RootActivityFactory
    {
        [SerializeField]
        private DebugSettings _settings;

        public override bool HasMotiveAssociated => false;

        public override void Generate(Mind mind, bool reinitMotive, out IRootActivity rootActivity)
        {
            rootActivity = new DebugHumanoid(mind as HumanoidMind, _settings);
        }

        [Serializable]
        public class DebugSettings
        {
            public GameObject DebugAnimationPrefab;
        }
    }
}
