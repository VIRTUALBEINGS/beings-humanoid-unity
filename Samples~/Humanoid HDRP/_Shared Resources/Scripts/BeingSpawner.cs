// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Linq;
using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.UnityIntegration;
using static VirtualBeings.Tech.UnityIntegration.BeingManager;

namespace VirtualBeings.Beings.Humanoid.Samples.Shared
{
    /// <summary>
    /// Sample script that spawn a the first being set in the BeingInstallerSettings of the scene.
    /// </summary>
    public class BeingSpawner : MonoBehaviour
    {
        Container Container => Container.Instance;

        private BeingManager _beingManager => Container.BeingManager;

        private Being _being;

        [SerializeField]
        private Transform[] _spawnPositions;

        void Start()
        {
            SpawnableBeings spawnableBeings = _beingManager.BeingManagerSettings.SpawnableBeings.ElementAt(0);

            for (int i = 0; i < Math.Min(spawnableBeings.BeingInstances.Count, _spawnPositions.Length); ++i)
            {
                BeingInstance beingInstance = spawnableBeings.BeingInstances.ElementAt(i);
                Transform     spawnPosition = _spawnPositions.ElementAt(i);

                if (beingInstance != null && spawnPosition != null)
                {
                    SpawnBeing(beingInstance, spawnableBeings.BeingSharedSettings, spawnPosition, i);
                }
            }
        }

        private void SpawnBeing(BeingInstance beingToSpawn, BeingSharedSettings sharedSettings, Transform spawnTransform, int id)
        {
            GameObject beingGO = Instantiate(beingToSpawn.BeingPrefab, spawnTransform.position, spawnTransform.rotation);

            _being = beingGO.GetComponent<Being>();
            BeingData beingData = new BeingData();
            beingData.AssignID(id);
            BeingInfo beingInfo = new(_being, beingData, beingToSpawn.BeingSettings, sharedSettings, beingToSpawn.Type);
            _beingManager.InitializeAndStartBeing(beingInfo);
        }
    }
}
