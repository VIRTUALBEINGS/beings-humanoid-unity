// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid
{
    [CreateAssetMenu(
        fileName = "New Humanoid Shared Client Settings",
        menuName = "VIRTUAL BEINGS/Humanoid Shared Client Settings",
        order = 1
    )]

    public class HumanoidSharedClientSettings : HumanoidSharedServerSettings
    {
        public override IFOVProvider FOVProvider => _FOVProvider;

        [Space, Header("FOV provider"), SerializeField]
        private float _maxDistFromOrigin = 2f;
        [SerializeField]
        private float _maxAngleFromForward = 135f;

        [SerializeField]
        private SphericalFOVProvider _FOVProvider;

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            _FOVProvider = new SphericalFOVProvider(_maxDistFromOrigin, _maxAngleFromForward);
        }
    }
}
