// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using VirtualBeings.BehaviorComposition.Activities;
using VirtualBeings.Beings.Humanoid;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.BehaviorComposition.Humanoids;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;
using Expression = VirtualBeings.Beings.Humanoid.Expression;

namespace VirtualBeings.Tech.Humanoid.Sample.Faces
{
    [Serializable]
    public class FacesSettings { }

    public class Faces : HumanoidRootActivity<Faces>
    {
        public FacesSettings FacesSettings { get; }

        private Stay       _stay;
        private Face       _face;
        private Locomotion _locomotion;
        private LookSimple _lookSimple;
        private LookAround _lookAround;

        public Faces(HumanoidMind parent, FacesSettings settings) : base(parent, null)
        {
            FacesSettings       = settings;
            OnCalculatePriority = () => Priority_7_Idle;

            Initialize += () => {
                _stay = new(this);

                float bodyWeight = Rand.Range(.02f, .06f);
                float headWeight = Rand.Range(.55f, .8f);

                _locomotion = new Locomotion(this);

                _lookSimple = new LookSimple(this, (IInteractable)null, true, true, 1.0f, null)
                {
                    BodyWeight01      = bodyWeight,
                    HeadWeight01      = headWeight,
                    ClampLookWeight01 = 0f,
                };

                _lookSimple.MaxDistractionDuration = 0f;

                _lookAround = new LookAround(
                    this,
                    firstPhaseDuration: 0.9f,
                    firstPhaseDurationVariation: 1.1f,
                    secondPhaseDuration: 0.8f,
                    secondPhaseDurationVariation: 1.1f,
                    maxYaw: 40f
                )
                {
                    BodyWeight01      = bodyWeight,
                    HeadWeight01      = headWeight,
                    ClampLookWeight01 = 0f,
                };

                _face = new Face(this);
            };
        }

        private IInteractable _closestInteractable;

        private IEnumerator FaceSample()
        {
            _stay.Start();
            _face.Start();

            // Wait one frame for the being to see all of the cubes around him
            yield return null;

            // _lookSimple.Start();
            // _lookSimple.RandomizeHeadLookDirection(false);
            // _lookSimple.RandomizeEyesLookDirection(false);

            _lookAround.Start();


            int i = 0;

            // Pick a point two meters in front of the character
            Vector3 walkTarget = Being.transform.position + 2.5f * Vector3.forward;

            _locomotion.Start(RSHumanoid.Walk, () => walkTarget);
            yield return new SuspendForDuration(1.7f);

            SetExpression(Expression.ASSERTIVE(FTTHumanoid.Rage, Rand.Range(0.5f, 0.7f), transitionVolume01: 0.4f));
            // SetExpression(Expression.ASSERTIVE(FTTHumanoid.Rage, 1f)); //Rand.Range(0.5f, 0.7f)));
            yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);
            yield return new SuspendForDuration(1f, 1.2f);
            SetExpression(Expression.NEUTRAL());
            yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);

            _stay.RequestST(STHumanoid.Shrug);
            yield return new SuspendForDuration(0.5f, 0.6f);

            SetExpression(Expression.SATISFIED(FTTHumanoid.Laugh, Rand.Range(0.3f, 0.4f)));
            yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);
            yield return new SuspendForDuration(0.8f, 1f);

            _lookSimple.Start(() => Camera.main.transform.position);
            // yield return new SuspendForDuration(0.6f, 0.7f);
            yield return new SuspendForDuration(0.3f);

            SetExpression(Expression.INTERESTED(FTTHumanoid.Exclamation, Rand.Range(0.4f, 0.5f)));
            yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);
            yield return new SuspendForDuration(1, 1.5f);

            while (FOREVER)
            {
                yield return null;

                // _lookSimple.DoFST(FSTHumanoid.RollingEyes, crossFadeDuration: 0.2f);
                // yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
                // yield return new SuspendForDuration(2);
                // _lookAround.DoFST(FSTHumanoid.HeadShake, crossFadeDuration: 0.2f);
                // yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);

                // ExpressionWrapper expression = ((i++) % 3) switch
                // {
                    // 1 => Expression.ASSERTIVE(FTTHumanoid.WithFOT),
                    // 2 => Expression.SATISFIED(FTTHumanoid.WithFOT),
                    // _ => Expression.NEUTRAL(FTTHumanoid.WithFOT),
                // };

                // SetExpression(expression);
                // yield return new SuspendForDuration(1);

                SetExpression(Expression.NEUTRAL());
                yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);
                // yield return new SuspendForDuration(1, 1.5f);

                // _lookAround.DoFST(FSTHumanoid.Rage, crossFadeDuration: 0.2f);
                // yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);
                // yield return new SuspendForDuration(1);

                // _lookAround.DoFST(FSTHumanoid.Laugh, crossFadeDuration: 0.2f);
                // yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);
                // yield return new SuspendForDuration(1);

                // _lookAround.DoFST(FSTHumanoid.GotIdea, crossFadeDuration: 0.2f);
                // yield return new SuspendWhile(() => _lookSimple.IsInAnimatedExpression);
                // yield return new SuspendForDuration(1);
            }
        }

        private IEnumerator TechShowcase()
        {
            _stay.Start();
            _face.Start();

            // Wait one frame for the being to see all of the cubes around him
            yield return null;

            _lookAround.Start();

            // Pick a point two meters in front of the character
            Vector3 walkTarget = Being.transform.position + 5f * Vector3.forward;

            float walkTime = 2f;

            const int JUST_WALK            = 0;
            const int WALK_THEN_SCREAM     = 1;
            const int WALK_THEN_SCREAM_UST = 2;
            const int WALK_INTERRUPTED     = 3;
            const int SIT_SEQUENCE         = 4;

            int animKind = JUST_WALK;

            switch (WALK_INTERRUPTED)
            {
                case JUST_WALK:
                {
                    yield return _locomotion.Start(RSHumanoid.Walk, () => walkTarget);
                } break;

                case WALK_THEN_SCREAM:
                {
                    _locomotion.Start(RSHumanoid.Walk, () => walkTarget);
                    yield return new SuspendForDuration(walkTime);

                    SetExpression(Expression.ASSERTIVE(FTTHumanoid.Rage, Rand.Range(0.5f, 0.7f), transitionVolume01: 0.4f));

                    yield return new SuspendForDuration(1.2f);
                    SetExpressionIntensity(0.2f);

                    yield return new SuspendForDuration(3);
                } break;

                case WALK_THEN_SCREAM_UST:
                {
                    _locomotion.Start(RSHumanoid.Walk, () => walkTarget);
                    yield return new SuspendForDuration(walkTime);

                    SetExpression(
                        Expression.ASSERTIVE(FTTHumanoid.Rage, Rand.Range(0.5f, 0.7f), transitionVolume01: 0.4f)
                    );
                    _locomotion.DoUST(USTHumanoid.Angry_Gesture, transitionSpeed01: 0.4f, overrideUpperBody01: 0f);

                    yield return new SuspendForDuration(1.2f);
                    SetExpressionIntensity(0.2f);

                    yield return new SuspendForDuration(3);
                } break;

                // With interruption
                case WALK_INTERRUPTED:
                {
                    _locomotion.Start(RSHumanoid.Walk, () => walkTarget);
                    yield return new SuspendForDuration(walkTime);

                    SetExpression(Expression.ASSERTIVE(FTTHumanoid.Rage, Rand.Range(0.5f, 0.7f), transitionVolume01: 0.4f));
                    _locomotion.DoUST(USTHumanoid.Angry_Gesture, transitionSpeed01: 0.4f, overrideUpperBody01: 0f);

                    yield return new SuspendForDuration(1.5f);

                    _lookSimple.Start(() => Camera.main.transform.position);
                    SetExpression(Expression.Confused());

                    _locomotion.TerminationRequested = true;

                    _locomotion.DoUST(
                        USTHumanoid.Suprised_CoveringMouth,
                        overrideUpperBody01: 1f
                    );

                    yield return new SuspendForDuration(1f);
                    SetExpression(Expression.NEUTRAL());

                    yield return new SuspendForDuration(0.6f);

                    SetExpression(Expression.SATISFIED(FTTHumanoid.Laugh, 0.3f));

                    yield return new SuspendForDuration(2f);
                } break;

                // It also works in stand / sit ?
                case SIT_SEQUENCE:
                {
                    _stay.Start(rs: RSHumanoid.Sit);
                    yield return new SuspendForDuration(walkTime);

                    SetExpression(
                        Expression.ASSERTIVE(FTTHumanoid.Rage, Rand.Range(0.5f, 0.7f), transitionVolume01: 0.4f)
                    );
                    // _stay.DoUST(USTHumanoid.Angry_Gesture, overrideUpperBody01: 1f);

                    yield return new SuspendForDuration(3);
                } break;

                case 5:
                {

                } break;
            }

            SetExpression(Expression.NEUTRAL());

            while (FOREVER)
            {
                yield return null;
            }
        }

        protected override IEnumerator MainProcess()
        {
            // yield return FaceSample();
            yield return TechShowcase();
        }
    }
}
