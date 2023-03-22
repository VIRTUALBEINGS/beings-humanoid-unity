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

namespace VirtualBeings.Beings.Humanoid.Factories
{
    public class FactoryDebugHumanoid : RootActivityFactory
    {
        [SerializeField]
        private DebugSettings _settings;

        public override void Generate(Mind mind, bool reinitMotive, out IRootActivity rootActivity, out Motive motive)
        {
            motive = null;
            rootActivity = new DebugHumanoid((HumanoidMind)mind, _settings);
        }

        [Serializable]
        public class DebugSettings
        {
            public GameObject DebugAnimationPrefab;
        }

        #if UNITY_EDITOR

        [MenuItem("Assets/Create/VirtualBeings/Humanoid Root Activities/Debug Animation")]
        public static void CreateMyAsset()
        {
            FactoryDebugHumanoid asset = CreateInstance<FactoryDebugHumanoid>();

            AssetDatabase.CreateAsset(asset, "Assets/Root Activity - Humanoid Debug Animation - New.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        #endif
    }
}
