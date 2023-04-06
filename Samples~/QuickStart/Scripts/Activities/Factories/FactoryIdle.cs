// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using UnityEditor;
using UnityEngine;
using VirtualBeings.BehaviorComposition;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    [CreateAssetMenu(
        fileName = "Humanoid Root Activity - Idle - New",
        menuName = "VIRTUAL BEINGS/Humanoid Root Activities/Idle",
        order = 1
    )]
    public class FactoryIdle : RootActivityFactory
    {
        [SerializeField]
        private IdleSettings _settings;

        public override bool HasMotiveAssociated => false;

        public override void Generate(
            Mind mind,
            bool reinitMotive,
            out IRootActivity floaterRootActivity,
            out Motive motive
        )
        {
            motive = null;
            floaterRootActivity = new Idle((HumanoidMind)mind, _settings);
        }
    }
}
