// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VirtualBeings.Beings.Humanoid;
using VirtualBeings.Tech.ActiveCognition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.BehaviorComposition.Humanoids;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Tech.Humanoid.Sample.Faces
{
    [Serializable]
    public class FacesSettings { }

    public class Faces : HumanoidRootActivity<Faces>
    {
        public override ExecutionType ExecutionType { get; protected set; } = ExecutionType.Default;

        public FacesSettings FacesSettings { get; }

        private Stay _stay;
        private Face _face;
        private LookSimple _lookSimple;
        private LookAround _lookAround;

        public Faces(IHumanoidActivity parent, FacesSettings settings) : base(parent, null)
        {
            FacesSettings = settings;
            OnCalculatePriority = () => Priority_7_Idle;

            Initialize += () =>
            {
                _stay = new (this);

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

                _face = new Face(this);
            };
        }

        private IInteractable _closestInteractable;

        protected override IEnumerator MainProcess()
        {
            _stay.Start();
            _face.Start();

            // Wait one frame for the being to see all of the cubes around him
            yield return null;

            _lookSimple.Start();

            while (FOREVER)
            {
                _lookSimple.DoFST(FSTHumanoid.Laugh, crossFadeDuration: 0.2f);
                yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
                yield return new SuspendForDuration(2);
                yield return null;
            }

            SetExpression(Expression.SHY(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.SATISFIED(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.INTERESTED(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);

            SetExpression(Expression.AVERSE(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.SAD(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.ASSERTIVE(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);

            SetExpression(Expression.Confused());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.PuffedCheeks());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.Grimace());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.EyesSquinted());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.EyebrowsRaise());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.SeriousLook());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.SmileSubtle());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);
            SetExpression(Expression.SmileWide());
            yield return new SuspendForDuration(2);
            SetExpression(Expression.NEUTRAL(FTTHumanoid.WithFOT));
            yield return new SuspendForDuration(2);

            _lookSimple.DoFST(FSTHumanoid.RollingEyes, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.MouthPuckerSides, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.Grimace, leftRight: -1f, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.Grimace, leftRight: 1f, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.PuffedCheeksBlow, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.Rage, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.SuspiciousLook, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.Wink, leftRight: -1f, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);
            _lookSimple.DoFST(FSTHumanoid.Wink, leftRight: 1f, crossFadeDuration: 0.2f);
            yield return new SuspendWhile(() => _lookSimple.IsInFOTorFST);
            yield return new SuspendForDuration(1);

            SetExpression(Expression.SATISFIED(FTTHumanoid.WithFOT, Rand.Range(0.4f, 0.6f), 1f));

            while (FOREVER)
            {
                yield return null;
            }
        }
    }
}
