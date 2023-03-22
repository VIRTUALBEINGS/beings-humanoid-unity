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
    public class MotiveIdle : MotiveName
    {
        public override string Name => "IDLE";
    }

    public class FactoryIdle : RootActivityFactory
    {
        [SerializeField]
        private IdleSettings _settings;

        public override MotiveName MotiveName => new MotiveIdle();

        public override void Generate(Mind mind, bool reinitMotive, out IRootActivity floaterRootActivity, out Motive motive)
        {
            motive = new Motive(mind, MotiveName, true, 1 / 200f, 1 / 2000f);
            ((HumanoidSelf)mind.Self).AddMotive(motive);
            floaterRootActivity = new Idle((HumanoidMind)mind, _settings);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/VirtualBeings/Humanoid Root Activities/Idle")]
        public static void CreateMyAsset()
        {
            FactoryIdle asset = CreateInstance<FactoryIdle>();

            AssetDatabase.CreateAsset(asset, "Assets/Humanoid Root Activity - Idle - New.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
#endif
    }
}
