// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Sample.LocomotionSample
{
    public class PillarCreator : MonoBehaviour
    {
        [SerializeField] private GameObject[] _cubePrefabs;
        [SerializeField] private float[] _cubePrefabProbabilities;
        [Space]
        [SerializeField] private int _minPillarHeight = 5;
        [SerializeField] private int _maxPillarHeight = 10;
        [Space]
        [SerializeField] private int _nbrPillarX = 3;
        [SerializeField] private int _nbrPillarZ = 3;
        [Space]
        [SerializeField] private float _spaceBetweenPillars = 5;
        [Space]
        [SerializeField, Range(0f, 1f)] private float _offsetPosFoundation01 = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _offsetPosEachPillar01 = 0.5f;
        [SerializeField, Range(0f, 90f)] private float _offsetYRotEachPillar = 20f;

        void Start()
        {
            float xMaxPosition = (_nbrPillarX * _spaceBetweenPillars) / 2f;
            float zMaxPosition = (_nbrPillarZ * _spaceBetweenPillars) / 2f;

            for (float x = -xMaxPosition + _spaceBetweenPillars / 2f; x < xMaxPosition; x += _spaceBetweenPillars)
            {
                float newX = x + Rand.Range((-_spaceBetweenPillars / 2f) * _offsetPosFoundation01, (_spaceBetweenPillars / 2f) * _offsetPosFoundation01);

                for (float z = -zMaxPosition + _spaceBetweenPillars / 2f; z < zMaxPosition; z += _spaceBetweenPillars)
                {
                    float newZ = z + Rand.Range((-_spaceBetweenPillars / 2f) * _offsetPosFoundation01, (_spaceBetweenPillars / 2f) * _offsetPosFoundation01);
                    float nbrPillarY = Rand.Range(_minPillarHeight, _maxPillarHeight + 1);

                    Vector3 raycastOrigin = new Vector3(newX, 10f, newZ) + transform.position;
                    float yOffset = 0f;

                    if (Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit hitInfo, raycastOrigin.y + 1f))
                    {
                        yOffset = hitInfo.point.y;

                        Misc.DrawDebugPoint(hitInfo.point, .2f, Color.white, 10f);
                    }

                    for (int y = 0; y < nbrPillarY; y++)
                    {
                        SpawnCube(newX, newZ, y + yOffset + .6f);
                    }
                }
            }
        }

        private void SpawnCube(float xPos, float zPos, float yHeight)
        {
            GameObject prefab = null;

            for (int n = 0; n < _cubePrefabProbabilities.Length; n++)
            {
                if (Rand.Below(_cubePrefabProbabilities[n]))
                {
                    prefab = _cubePrefabs[n];
                    break;
                }
            }

            GameObject cube = Instantiate(prefab ?? _cubePrefabs[^1], transform);
            Collider cubeCollider = cube.GetComponentInChildren<Collider>();
            float xSize = cubeCollider.bounds.extents.x;
            float ySize = cubeCollider.bounds.size.y;
            float zSize = cubeCollider.bounds.extents.z;

            // Position
            xPos += Rand.sign * Rand.Range(0, _offsetPosEachPillar01 * xSize);
            zPos += Rand.sign * Rand.Range(0, _offsetPosEachPillar01 * zSize);

            cube.transform.position = new Vector3(xPos, yHeight * ySize, zPos) + transform.position;
            Misc.DrawDebugPoint(new Vector3(xPos, yHeight * ySize, zPos) + transform.position, .2f, Color.green, 10f);
        }
    }
}
