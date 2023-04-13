// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEditor;
using UnityEngine;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid
{
    [CustomEditor(typeof(HumanoidBeing))]
    public class HumanoidBeingEditor : BeingEditor
    {
        protected override PostProcessAnimation CreatePostProcessAnimationSpecific()
        {
            PostProcessAnimationHumanoid result = new PostProcessAnimationHumanoid();
            return result;
        }
    }
}