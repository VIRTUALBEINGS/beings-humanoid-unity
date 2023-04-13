// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Linq;
using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.UnityIntegration;
using static VirtualBeings.Tech.UnityIntegration.BeingManager;

namespace VirtualBeings.Beings.Humanoid.Sample.QuickStart
{
    /// <summary>
    /// Sample script that spawn a the first being set in the BeingInstallerSettings of the scene.
    /// </summary>
    public class BeingSpawner : MonoBehaviour
    {
        Container Container => Container.Instance;

        private BeingManager _beingManager => Container.BeingManager;

        private Being _being;

        void Start()
        {
            SpawnableBeings spawnableBeings = _beingManager.BeingManagerSettings.SpawnableBeings.ElementAt(0);
            BeingInstance beingInstance = spawnableBeings.BeingInstances.ElementAt(0);
            SpawnBeing(beingInstance, spawnableBeings.BeingSharedSettings);       
        }

        private void SpawnBeing(BeingInstance beingToSpawn, BeingSharedSettings sharedSettings)
        {
            GameObject beingGO = Instantiate(beingToSpawn.BeingPrefab, Vector3.zero, Quaternion.identity);

            _being = beingGO.GetComponent<Being>();

            BeingInfo beingInfo = new(_being, new BeingData(), beingToSpawn.BeingSettings, sharedSettings, beingToSpawn.Type);
            _beingManager.InitializeAndStartBeing(beingInfo);
        }
    }
}
