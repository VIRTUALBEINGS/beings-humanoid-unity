// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections;
using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.BehaviorComposition.Humanoids;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;
using VirtualBeings.Tech.Utils;

namespace VirtualBeings.Beings.Humanoid.Samples.DanceSample
{
    [Serializable]
    public class DanceSettings
    {
    }

    public class Dance : HumanoidRootActivity<Dance>
    {
        private class StartDanceEvent : VBEvent { }

        public Dance(HumanoidMind parent, DanceSettings settings) : base(parent, null)
        {
            Initialize += () =>
            {
                _stay = new(this);
                _locomotion = new Locomotion(this);
                _lookDuringMoveTo = new LookDuringMoveTo(
                    this,
                    _locomotion,
                    emoter: null,
                    minPitch: -5f,
                    maxPitch: 5f,
                    maxYaw: 45f,
                    averageDuration: 2f,
                    durationVariationFactor: 1.25f
                );

                _lookAround = new LookAround(this);
                _lookSimple = new LookSimple(this, Mind.PlayerTracker.MainPlayer, true, true, 0f, null);
                _emoteDuringDance = new EmoteDuringDance(this);
            };
        }

        private Stay _stay;
        private Locomotion _locomotion;
        private LookAround _lookAround;
        private LookSimple _lookSimple;
        private LookDuringMoveTo _lookDuringMoveTo;
        private EmoteDuringDance _emoteDuringDance;

        protected override IEnumerator MainProcess()
        {
            int id = Being.BeingID;
            float[] delays = { 1f, 1f, 1f };
            Vector3[] targets = { new(-1.4f, 0, 0.5f), new(1.5f, 0, 0.7f), new(.0f, 0, -.8f), };
            RSHumanoid[] dances = { RSHumanoid.Dance_Samba, RSHumanoid.Dance_Robot, RSHumanoid.Dance_Wave, };
            ExpressionWrapper[] exprs = { Expression.ASSERTIVE(.3f), Expression.SATISFIED(.4f), Expression.ASSERTIVE(0.5f), };

            _lookSimple.Start();
            _lookAround.Start();
            SetExpression(exprs[id]);

            yield return new SuspendForDuration(delays[id]);

            _stay.Start();
            _lookDuringMoveTo.Start();

            _stay.RequestST(STHumanoid.Laugh);
            yield return new SuspendWhile(() => _stay.IsInTransition);

            yield return _locomotion.Start(RSHumanoid.Walk, () => targets[id], allowAvoidance: false);

            _lookDuringMoveTo.Interrupt();
            yield return new SuspendForDuration(.2f, .3f);

            _emoteDuringDance.Start(id);

            if (id == 2)
                Being.WorldEvents.Raise(new StartDanceEvent());
            else
            {
                bool signalReceived = false;
                Being.WorldEvents.AddListener<StartDanceEvent>((_) => signalReceived = true);

                if (id == 0)
                {
                    yield return new SuspendForDuration(.4f, .6f);

                    _stay.DoUST(USTHumanoid.WipeForehead, .4f, 1f, Rand.sign);
                    yield return new SuspendWhile(() => _stay.IsInUST, Rand.Range(.2f, .25f));

                    _stay.DoUST(USTHumanoid.TwistTorso_Subtle, .6f, 1f, Rand.sign);
                }
                else
                {
                    //yield return new SuspendForDuration(.4f, .6f);

                    _stay.DoUST(USTHumanoid.ApplauseQuick, .5f, 1f, Rand.sign);
                }

                yield return new SuspendUntil(() => signalReceived);
            }

            _stay.SetTargetRS(dances[id], TransitionTypeHumanoid.Default);

            float timeOfDanceStart = Time.time;
            const float firstPhase = 14.85f;
            const float pause = 2.6f;

            yield return new SuspendForDuration(firstPhase);
            _stay.SetTargetRS(RSHumanoid.Stand, TransitionTypeHumanoid.Default);
            _emoteDuringDance.Interrupt();

            yield return new SuspendForDuration(1.2f);
            _lookAround.Interrupt();
            _lookSimple.HeadWeight01 = .8f;
            _lookSimple.EyeWeight01 = 1f;
            float signedAngleToCam = Being.SignedRootAngleOnXZTo(Camera.main.transform.position);
            _stay.RequestST(STHumanoid.Turn, .6f, 1f, signedAngleToCam / 90f);
            yield return new SuspendWhile(() => _stay.IsInTransition && Time.time < timeOfDanceStart + firstPhase + pause);

            _stay.SetTargetRS(RSHumanoid.Dance_Robot, TransitionTypeHumanoid.Default);
            _emoteDuringDance.Start(0);

            while (FOREVER)
                yield return null;
        }

        private class EmoteDuringDance : HumanoidActivity<EmoteDuringDance>
        {
            private readonly SuspendForDuration _suspend;
            private readonly LookSimple _lookSimple;

            private int _beingID;

            public EmoteDuringDance(IHumanoidActivity parent) : base(parent, null)
            {
                _suspend = new SuspendForDuration();
                _lookSimple = new LookSimple(this, (IInteractable)null, true, true, 0f, null);
            }

            public EmoteDuringDance Start(int beingID)
            {
                _beingID = beingID;
                return base.Start();
            }

            protected override IEnumerator MainProcess()
            {
                _lookSimple.Start();

                switch (_beingID)
                {
                    case 0:
                        while (FOREVER)
                        {
                            SetExpression(Expression.ASSERTIVE(Rand.Range(.2f, .3f)));
                            yield return _suspend.Renew(1.2f, 2f);

                            SetExpression(Expression.SATISFIED(Rand.Range(.2f, .3f)));
                            yield return _suspend.Renew(1.2f, 2f);
                        }
                    default:
                        while (FOREVER)
                        {
                            SetExpression(Expression.INTERESTED(Rand.Range(.6f, .7f)));
                            yield return _suspend.Renew(1.2f, 2f);

                            SetExpression(Expression.SATISFIED(Rand.Range(.5f, .7f)));
                            yield return _suspend.Renew(1.2f, 2f);
                        }
                    case 2:
                        SetExpression(Expression.ASSERTIVE(Rand.Range(.8f, 1f)));
                        yield return _suspend.Renew(.5f, .8f);

                        while (FOREVER)
                        {
                            SetExpression(Expression.AVERSE(Rand.Range(.5f, 1f)));
                            yield return _suspend.Renew(.5f, .8f);

                            SetExpression(Expression.SATISFIED(Rand.Range(.5f, 1f)));
                            yield return _suspend.Renew(1.2f, 2f);

                            SetExpression(Expression.ASSERTIVE(Rand.Range(.5f, 1f)));
                            yield return _suspend.Renew(1.2f, 2f);
                        }
                }
            }
        }
    }
}
