// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using UnityEngine;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Samples.Shared
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VisualizePathfinding_))]
    public class VisualizeNavMesh : MonoBehaviour
    {
        [SerializeField]
        private int _wallHeight, _wallWidth, _wallDepth;
        [SerializeField]
        private GameObject[] _cubePrefabs;
        [SerializeField]
        private float[] _cubePrefabProbabilities;
        [SerializeField]
        private float _range;
        [SerializeField]
        private Transform _smashingBall;
        [SerializeField]
        private float _thresholdDistanceToSmashingBall = 5;

        private Rigidbody[] _cubes;
        private Vector3[] _cubeVelocities, _cubeAngularVelocities;

        private IEnumerator Start()
        {
            _cubes = new Rigidbody[_wallHeight * _wallWidth * _wallDepth];
            _cubeVelocities = new Vector3[_cubes.Length];
            _cubeAngularVelocities = new Vector3[_cubes.Length];

            for (int i = 0; i < _wallHeight; i++)
            {
                for (int j = 0; j < _wallWidth; j++)
                {
                    for (int k = 0; k < _wallDepth; k++)
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

                        GameObject cube = Instantiate(prefab ?? _cubePrefabs[^1],
                            transform.TransformPoint(new Vector3(
                                k * (1f + _range) + Rand.Range(-_range, _range) - _wallDepth * .5f,
                                i + .5f,
                                j - _wallWidth * .5f)),
                            transform.rotation, transform);

                        _cubes[i * _wallWidth * _wallDepth + j * _wallDepth + k] = cube.GetComponent<Rigidbody>();
                    }
                }
            }

            // wait for smashing ball being close
            while ((transform.position - _smashingBall.position).sqrMagnitude > _thresholdDistanceToSmashingBall * _thresholdDistanceToSmashingBall)
                yield return null;

            yield return new WaitForSeconds(4f);

            VisualizePathfinding_ visualizePathfinding = GetComponent<VisualizePathfinding_>();
            Rigidbody rbSmashingBall = _smashingBall.GetComponent<Rigidbody>();

            while (true)
            {
                Vector3 sphereVelocity = rbSmashingBall.velocity;
                Vector3 sphereAngularVelocity = rbSmashingBall.angularVelocity;
                rbSmashingBall.isKinematic = true;

                for (int i = 0; i < _cubes.Length; i++)
                {
                    _cubeVelocities[i] = _cubes[i].velocity;
                    _cubeAngularVelocities[i] = _cubes[i].angularVelocity;
                    _cubes[i].isKinematic = true;
                }

                yield return new WaitForSeconds(1f);

                visualizePathfinding.DoPathTraversal(4f);
                yield return new WaitForSeconds(1000f);
                //yield return new WaitForSeconds(.5f);

                rbSmashingBall.isKinematic = false;
                rbSmashingBall.AddForce(sphereVelocity, ForceMode.VelocityChange);
                rbSmashingBall.AddTorque(sphereAngularVelocity, ForceMode.VelocityChange);

                for (int i = 0; i < _cubes.Length; i++)
                {
                    _cubes[i].isKinematic = false;
                    _cubes[i].AddForce(_cubeVelocities[i], ForceMode.VelocityChange);
                    _cubes[i].AddTorque(_cubeAngularVelocities[i], ForceMode.VelocityChange);
                }

                yield return new WaitForSeconds(.9f);
            }
        }
    }
}
