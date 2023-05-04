// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualBeings.Beings.Humanoid.Samples.Shared;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Sample.Faces
{
    public class CameraController : MonoBehaviour
    {
        Container Container => Container.Instance;

        private List<Being> _beings = new();
        private CameraFade _cameraFade;
        private int _indexBeingAttached = 0;
        private bool _onFadeInFinished;

        private void Awake()
        {
            _cameraFade = GetComponentInChildren<CameraFade>();
            _cameraFade.OnFadeInFinished += OnFadeInFinished;

            Container.WorldEvents.AddListener<WorldEvent_InteractableRegistration<Being>>(OnBeingRegistration);
            Container.WorldEvents.AddListener<WorldEvent_InteractableUnregistration<Being>>(OnBeingUnregistration);
        }

        private IEnumerator Start()
        {
            Animator animator = GetComponentInChildren<Animator>();

            Animator interactable1Animator = GameObject.Find("Interactable Cube 1").GetComponentInChildren<Animator>(true);
            Animator interactable2Animator = GameObject.Find("Interactable Cube 2").GetComponentInChildren<Animator>(true);
            Animator interactable3Animator = GameObject.Find("Interactable Cube 3").GetComponentInChildren<Animator>(true);

            string[] triggers = new string[]
            {
                "Trigger1",
                "Trigger2",
            };

            int indexTrigger = 0;

            while (_beings.Count == 0)
                yield return null;

            while (true)
            {
                // start a new phase
                AttachToNextBeing();
                _onFadeInFinished = false;

                animator.SetTrigger(triggers[indexTrigger]);
                interactable1Animator.SetTrigger(triggers[indexTrigger]);
                interactable2Animator.SetTrigger(triggers[indexTrigger]);
                interactable3Animator.SetTrigger(triggers[indexTrigger]);
                indexTrigger = (indexTrigger + 1) % triggers.Length;

                // let camera move a bit
                yield return new WaitForSeconds(Rand.Range(8f, 12f));

                // fade and wait for black screen
                _cameraFade.DoFade(Rand.Range(2f, 2.5f));

                while (!_onFadeInFinished)
                    yield return null;
            }
        }

        private void OnFadeInFinished()
        {
            _onFadeInFinished = true;
        }

        private void OnBeingRegistration(WorldEvent_InteractableRegistration<Being> evt)
        {
            Being being = evt.Interactable;

            if(!_beings.Contains(being))
                _beings.Add(being);
        }

        private void OnBeingUnregistration(WorldEvent_InteractableUnregistration<Being> evt)
        {
            _beings.Remove(evt.Interactable);
        }

        private void AttachToNextBeing()
        {
            if (_beings.Count > 0)
            {
                _indexBeingAttached = (_indexBeingAttached + 1) % _beings.Count;
                Being beingToBeAttached = _beings[_indexBeingAttached];

                transform.position = new Vector3(beingToBeAttached.RootPosition.x, transform.position.y, beingToBeAttached.RootPosition.z);
                transform.rotation = beingToBeAttached.RootRotation;
            }
        }
    }

}
