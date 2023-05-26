// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;

namespace VirtualBeings.Beings.Humanoid
{
    [Serializable]
    [CreateAssetMenu(
        fileName = "New Humanoid Settings",
        menuName = "VIRTUAL BEINGS/Humanoid Settings",
        order = 1
    )]
    public class HumanoidSettings : HumanoidBeingSettingsBase
    {
        public override void InitializeAnimationStateBase()
        {
            _ = BodyAttitudeHumanoid.None;
            _ = BodyPoseHumanoid.None;

            _ = FSHumanoid.None;
            _ = FSTHumanoid.None;
            _ = FTTHumanoid.Default;

            _ = RSHumanoid.None;
            _ = STHumanoid.None;
            _ = TransitionTypeHumanoid.Default;
        }
    }
}
