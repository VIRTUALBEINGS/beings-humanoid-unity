// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualBeings.Tech.Shared;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Samples.Shared
{
    [DisallowMultipleComponent]
    public class VisualizePathfinding_ : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _startPositions, _goalPositions;
        [SerializeField]
        private GameObject _pathVisualizerPrefab, _lineSegmentVisualizerPrefab;
        [SerializeField]
        private float _clearance = .5f, _delayBetweenPathfinds = .1f, _yOfSegments = .2f;

        private Transform[] _pathVisualizers;
        private LineSegmentVisualizer _visualizer;
        private bool _doPathTraversal;
        private float _durationOfPathTraversal;

        public void DoPathTraversal(float durationOfPathTraversal)
        {
            _doPathTraversal = true;
            _durationOfPathTraversal = durationOfPathTraversal;
        }

        private IEnumerator Start()
        {
            _visualizer = Instantiate(_lineSegmentVisualizerPrefab, transform).GetComponent<LineSegmentVisualizer>();
            _pathVisualizers = new Transform[_startPositions.Length];

            for (int i = 0; i < _startPositions.Length; i++)
            {
                _pathVisualizers[i] = Instantiate(_pathVisualizerPrefab, _startPositions[i].position,
                    Quaternion.identity, transform).GetComponent<Transform>();
            }

            yield return new WaitForSeconds(.5f);

            StartCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            List<Vector3> buffer = new();
            List<Vector3>[] paths = new List<Vector3>[_startPositions.Length];

            for (int i = 0; i < _startPositions.Length; i++)
                paths[i] = new();

            NavigableTerrain navigableTerrain = FindObjectOfType<NavigableTerrain>();
            WaitForSeconds waitForSeconds = new(_delayBetweenPathfinds);
            Vector3 yVector = new(0f, _yOfSegments, 0f);

            while (true)
            {
                _visualizer.Clear();

                for (int i = 0; i < _startPositions.Length; i++)
                {
                    Vector3 start = _startPositions[i].position;
                    Vector3 goal = _goalPositions[i].position;

                    navigableTerrain.GetDebugPath(start, goal, _clearance, buffer);

                    for (int j = 0; j < buffer.Count; j++)
                    {
                        _visualizer.AddSegment((j == 0 ? start : buffer[j - 1]) + yVector, buffer[j] + yVector, 2);
                    }

                    if (_doPathTraversal)
                    {
                        paths[i].Clear();
                        paths[i].Add(start);
                        paths[i].AddRange(buffer);
                    }
                }

                if (_doPathTraversal)
                {
                    _doPathTraversal = false;

                    for (int i = 0; i < _startPositions.Length; i++)
                        StartCoroutine(TraversePath(_pathVisualizers[i], paths[i]));

                    yield return new WaitForSeconds(_durationOfPathTraversal);
                }

                yield return waitForSeconds;
            }
        }

        private IEnumerator TraversePath(Transform tr, List<Vector3> path)
        {
            float totalLength = 0f;

            for (int i = 0; i < path.Count - 1; i++)
                totalLength += Vector3.Distance(path[i], path[i + 1]);

            while (true)
            {
                float duration = _durationOfPathTraversal * Rand.Range(.6f, 1.4f) * .5f;
                float timeOfStart = Time.time;
                float t = 0f;

                while (t < 1f)
                {
                    t = (Time.time - timeOfStart) / duration;
                    float targetDistance = t * totalLength;
                    float segmentLength = 0f;
                    Vector3 p0 = Vector3.zero;
                    Vector3 p1 = Vector3.zero;

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        segmentLength = Vector3.Distance(path[i], path[i + 1]);

                        if (i == path.Count - 2 || targetDistance <= segmentLength)
                        {
                            p0 = path[i];
                            p1 = path[i + 1];
                            break;
                        }
                        else
                        {
                            targetDistance -= segmentLength;
                        }
                    }

                    tr.position = Vector3.Lerp(p0, p1, targetDistance / segmentLength);
                    yield return null;
                }

                timeOfStart = Time.time;

                while (t > 0f)
                {
                    t = 1f - (Time.time - timeOfStart) / duration;
                    float targetDistance = t * totalLength;
                    float segmentLength = 0f;
                    Vector3 p0 = Vector3.zero;
                    Vector3 p1 = Vector3.zero;

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        segmentLength = Vector3.Distance(path[i], path[i + 1]);

                        if (i == path.Count - 2 || targetDistance <= segmentLength)
                        {
                            p0 = path[i];
                            p1 = path[i + 1];
                            break;
                        }
                        else
                        {
                            targetDistance -= segmentLength;
                        }
                    }

                    tr.position = Vector3.Lerp(p0, p1, targetDistance / segmentLength);
                    yield return null;
                }
            }
        }
    }
}
