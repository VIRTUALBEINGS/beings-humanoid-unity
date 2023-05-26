// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.BehaviorComposition.Humanoids;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Samples.LookSample
{
    [Serializable]
    public class LookSettings { }

    public class Look : HumanoidRootActivity<Look>
    {
        public LookSettings LookSettings { get; }

        private Stay _stay;
        private Face _face;
        private LookSimple _lookSimple;
        private LookAround _lookAround;

        public Look(HumanoidMind parent, LookSettings settings) : base(parent, null)
        {
            LookSettings = settings;
            OnCalculatePriority = () => Priority_7_Idle;

            Initialize += () =>
            {
                _stay = new Stay(this);

                float bodyWeight = Rand.Range(.02f, .06f);
                float headWeight = Rand.Range(.55f, .8f);

                _lookSimple = new LookSimple(this, (IInteractable)null, true, true, 1.0f, null)
                {
                    BodyWeight01 = bodyWeight,
                    HeadWeight01 = headWeight,
                    ClampLookWeight01 = 0f,
                };

                _lookSimple.MaxDistractionDuration = 0f;

                _lookAround = new LookAround(this, firstPhaseDuration: .8f, firstPhaseDurationVariation: 1.1f, maxYaw: 20f)
                {
                    BodyWeight01 = bodyWeight,
                    HeadWeight01 = headWeight,
                    ClampLookWeight01 = 0f,
                };

                _lookAround.OnNewLookTarget += (_, _) => OnLookTargetModification();

                _face = new Face(this);
            };
        }

        private IInteractable _closestInteractable;

        bool IsInCameraFocus() => Being.DistanceOnXZTo(Camera.main.transform.position) < 2f;

        protected override IEnumerator MainProcess()
        {
            _stay.Start();
            _face.Start();

            // Wait one frame for the being to see all of the cubes around him
            yield return null;

            List<TrackedInteractable> trackedInteractables =
                Being.Mind.InteractableMemory.FindAll(t => t.Interactable is InteractableController);

            _closestInteractable = trackedInteractables
                                  .OrderBy(t => Vector3.Distance(Being.RootPosition, t.Interactable.RootPosition))
                                  .FirstOrDefault()?.Interactable;

            while (FOREVER)
            {
                _lookSimple.Interrupt();
                _lookAround.Interrupt();

                while (!IsInCameraFocus())
                {
                    yield return null;
                }

                yield return PhaseOne();
                yield return PhaseTwo();
                yield return PhaseThree();
            }
        }

        private float _remainingTime;

        private IEnumerator PhaseOne()
        {
            ((InteractableController)_closestInteractable).gameObject.SetActive(false);

            yield return new SuspendForDuration(0.25f, 0.5f);

            _lookSimple.Start(() => Camera.main.transform.position);
            OnLookTargetModification();

            yield return new SuspendForDuration(.3f, 1.5f);

            PlayST();

            //yield return new SuspendWhile(() => _locomotion.IsInST() || _locomotion.IsInUST());
            //yield return new SuspendForDuration(0.25f, 0.5f);
            yield return new SuspendWhile(() => _stay.IsInST || _stay.IsInUST, 0f, 0f, Rand.Range(1.5f, 2.5f));
        }

        private IEnumerator PhaseTwo()
        {
            ((InteractableController)_closestInteractable).gameObject.SetActive(true);

            yield return new SuspendForDuration(0.15f, 0.18f);

            _lookSimple.Start(_closestInteractable);

            yield return new SuspendForDuration(.3f, .6f);

            FSHumanoid satisfied = FSHumanoid.SATISFIED;
            float intensity = _randomExpressionsIntensity[satisfied]();
            float duration = Rand.Range(0.5f, 1.25f);
            _face.DoExpression(satisfied, duration, intensity);

            yield return new SuspendForDuration(1.5f, 2.2f);
        }

        private IEnumerator PhaseThree()
        {
            _lookAround.Start(_closestInteractable.SalientPosition);
            yield return null;

            ((InteractableController)_closestInteractable).gameObject.SetActive(false);

            // Be surprised 1/3 of the time
            if (/*Rand.Below(0.3f)*/true)
            {
                yield return new SuspendForDuration(0.1f, 0.2f);

                FSHumanoid confused = FSHumanoid.AVERSE;
                float intensity = _randomExpressionsIntensity[confused]();
                float duration = Rand.Range(0.5f, 1.0f);
                _face.DoExpression(confused, duration, intensity);
            }

            yield return new SuspendWhile(IsInCameraFocus);
        }

        private void PlayST()
        {
            var usts = new USTHumanoid[]
            {
                USTHumanoid.SwingArms_Subtle,
                USTHumanoid.HeadTilt_Stretch,
                USTHumanoid.TwistTorso_Subtle,
            };

            _stay.DoUST(usts[Rand.Range(0, usts.Length)], transitionLeftRight: Rand.sign);
        }

        private void OnLookTargetModification()
        {
            if (Rand.Above(0.5f))
            {
                FSHumanoid randomExpression = _randomExpressions[Rand.Range(0, _randomExpressions.Length)];
                float intensity = _randomExpressionsIntensity[randomExpression]();
                float duration = Rand.Range(0.5f, 1.25f);
                _face.DoExpression(randomExpression, duration, intensity);
            }
        }

        private FSHumanoid[] _randomExpressions =
        {
            FSHumanoid.INTERESTED,
            FSHumanoid.SATISFIED,
            FSHumanoid.SHY,
        };

        private Dictionary<FSHumanoid, Func<float>> _randomExpressionsIntensity = new()
        {
            [FSHumanoid.AVERSE] = () => Rand.Range(0.1f, 0.2f),
            [FSHumanoid.INTERESTED] = () => Rand.Range(0.1f, 0.3f),
            [FSHumanoid.SHY] = () => Rand.Range(0.1f, 0.3f),
            [FSHumanoid.SATISFIED] = () => Rand.Range(0.1f, 0.25f),
        };
    }
}
