// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid
{
    public class HumanoidSharedClientSettings : HumanoidSharedServerSettings, ISerializationCallbackReceiver
    {
        public override IFOVProvider FOVProvider => _FOVProvider;

        public override Dictionary<int, IVocalAnimationList> DictVocalAnimations => _dictVocalAnimations;

        [Space, Header("FOV provider"), SerializeField]
        private float _maxDistFromOrigin = 2f;
        [SerializeField]
        private float _maxAngleFromForward = 135f;

        [SerializeField]
        private SphericalFOVProvider _FOVProvider;

        private Dictionary<int, IVocalAnimationList> _dictVocalAnimations;

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            _FOVProvider = new SphericalFOVProvider(_maxDistFromOrigin, _maxAngleFromForward);

            _dictVocalAnimations = new Dictionary<int, IVocalAnimationList>();
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/VirtualBeings/HumanoidSharedClientSettings")]
        public static void CreateMyAsset()
        {
            HumanoidSharedClientSettings asset = CreateInstance<HumanoidSharedClientSettings>();

            AssetDatabase.CreateAsset(asset, "Assets/NewHumanoidSharedClientSettings.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
#endif
    }
}
