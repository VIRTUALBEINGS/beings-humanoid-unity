﻿// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using UnityEngine;
using VirtualBeings.BehaviorComposition.Activities;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Shared;

namespace VirtualBeings.Tech.Beings.Humanoid
{
    public class DebugHumanoid : HumanoidRootActivity<DebugHumanoid>
    {
        public FactoryDebugHumanoid.DebugSettings DebugSettings { get; }

        private ActuatorViewer _actuatorViewer;
        private DebugAnimationUI _debugAnimationUI;
        private GameObject _instanceDebugUI;

        public DebugHumanoid(HumanoidMind parent, FactoryDebugHumanoid.DebugSettings settings)
            : base(parent, null)
        {
            DebugSettings = settings;

            Initialize += () => {
                _actuatorViewer = new ActuatorViewer(this);
            };
        }

        protected override void Enter()
        {
            base.Enter();

            _instanceDebugUI = GameObject.Instantiate(DebugSettings.DebugAnimationPrefab, Being.gameObject.transform);
            _debugAnimationUI = _instanceDebugUI.GetComponent<DebugAnimationUI>();
            _debugAnimationUI.ActuatorViewer = _actuatorViewer;
        }

        protected override void Exit()
        {
            base.Exit();

            GameObject.Destroy(_instanceDebugUI);
        }

        protected override IEnumerator MainProcess()
        {
            yield return _actuatorViewer.Start();
        }
    }
}
