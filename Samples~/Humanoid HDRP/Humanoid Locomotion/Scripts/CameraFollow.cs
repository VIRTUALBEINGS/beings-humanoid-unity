// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualBeings.Beings.Humanoid.Sample.Look;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Sample.LocomotionSample
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float zOffset = -12;
        [SerializeField] private Camera _mainCamera, _secondaryCamera;

        private Container Container => Container.Instance;
        private List<Being> _beings = new();

        private CameraFade _cameraFade;
        private int _indexBeingAttached = 0;
        private bool _onFadeInFinished;
        private Being _currentBeingAttached = null;
        private bool _doSecondaryCameraNext;

        private void Awake()
        {
            _cameraFade = GetComponentInChildren<CameraFade>();
            _cameraFade.OnFadeInFinished += OnFadeInFinished;
        }

        private IEnumerator Start()
        {
            _beings.AddRange(Container.BeingManager.Beings);

            Container.WorldEvents.AddListener<WorldEvent_OnBeingSpawn>(RegisterBeing);
            Container.WorldEvents.AddListener<WorldEvent_OnBeingUnspawn>(UnregisterBeing);

            Animator animator = GetComponentInChildren<Animator>();

            while (_beings.Count == 0)
                yield return null;

            while (true)
            {
                // start a new phase
                AttachToNextBeing();
                _onFadeInFinished = false;

                // let camera move a bit
                yield return new WaitForSeconds(Rand.Range(3f, 5f));

                // fade and wait for black screen
                _cameraFade.DoFade(Rand.Range(.5f, .8f));

                while (!_onFadeInFinished)
                    yield return null;
            }
        }

        private void Update()
        {
            if (_currentBeingAttached != null)
            {
                transform.position = new Vector3(_currentBeingAttached.RootPosition.x, transform.position.y, _currentBeingAttached.RootPosition.z);
            }
        }

        private void RegisterBeing(WorldEvent_OnBeingSpawn evt)
        {
            _beings.Add(evt.Being);
        }

        private void UnregisterBeing(WorldEvent_OnBeingUnspawn evt)
        {
            _beings.Remove(evt.Being);
        }

        private void OnFadeInFinished()
        {
            _onFadeInFinished = true;
        }

        private void AttachToNextBeing()
        {
            if (_beings.Count > 0)
            {
                _indexBeingAttached = (_indexBeingAttached + 1) % _beings.Count;
                _currentBeingAttached = _beings[_indexBeingAttached];
                transform.position = new Vector3(_currentBeingAttached.RootPosition.x, transform.position.y, _currentBeingAttached.RootPosition.z);
            }

            _mainCamera.gameObject.SetActive(!_doSecondaryCameraNext);
            _secondaryCamera.gameObject.SetActive(_doSecondaryCameraNext);

            _cameraFade.OnFadeInFinished -= OnFadeInFinished;
            _cameraFade = GetComponentInChildren<CameraFade>();
            _cameraFade.OnFadeInFinished += OnFadeInFinished;

            _doSecondaryCameraNext = !_doSecondaryCameraNext;
        }
    }
}
