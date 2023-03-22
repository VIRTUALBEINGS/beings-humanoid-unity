// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEditor;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid
{
    [CustomEditor(typeof(PostProcessAnimationHumanoid))]
    public class PostProcessAnimationHumanoidEditor : PostProcessAnimationEditor
    {
        public override AgentType AgentType => AgentType.Humanoid;

    }

}