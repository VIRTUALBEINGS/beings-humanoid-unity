// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections;
using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Sample.QuickStart
{
    [Serializable]
    public class ExploreSettings { }

    public class Explore : HumanoidRootActivity<Explore>
    {
        public override ExecutionType ExecutionType { get; protected set; } = ExecutionType.Default;

        public ExploreSettings ExploreSettings { get; }

        public Explore(IHumanoidActivity parent, ExploreSettings exploreSettings) : base(parent, null)
        {
            ExploreSettings = exploreSettings;
            OnCalculatePriority = () => Priority_7_Idle;
           
            Initialize += () =>
            {
                _goal = new Vector3(0f, 0f, 0f);
                _locomotion = new(this);
                _stay = new(this);
                _lookSimple = new LookSimple(this, targetIInteractable: null, false, false, 0.7f, null);
                _face = new(this);
            };
        }

        protected override IEnumerator MainProcess()
        {
            _stay.Start();
            _lookSimple.Start();
            _face.Start();

            while (FOREVER)
            {
                float a = Rand.Range(0f, 360);
                _goal = Vector3.zero + new Vector3(Mathf.Sin(a), 0f, Mathf.Cos(a)) * Rand.Range(2f, 10f);

                FSHumanoid randomExpression = _randomExpressions[Rand.Range(0, _randomExpressions.Length)];
                _face.DoExpression(randomExpression, Rand.Range(2f, 4f), Rand.Range(0.5f, 1f));

                yield return _locomotion.Start(RSHumanoid.Walk, () => _goal);

                yield return new SuspendForDuration(Rand.Range(0.5f, 1f));

                if(Rand.Bool)
                {
                    STHumanoid randomST = _randomSTs[Rand.Range(0, _randomSTs.Length)];
                    _stay.RequestST(randomST, transitionLeftRight: Rand.sign); // Some ST ignore left / right
                }
                else
                {
                    USTHumanoid randomUST = _randomUSTs[Rand.Range(0, _randomUSTs.Length)];
                    _locomotion.DoUST(randomUST, transitionLeftRight: Rand.sign); // Some UST ignore left / right
                }

                yield return new SuspendWhile(() => _stay.IsInTransition());
            }
        }

        private STHumanoid[] _randomSTs = new STHumanoid[]
        {
            STHumanoid.InspectCurious,
            STHumanoid.StretchArms,
            STHumanoid.Shrug,
            STHumanoid.Defeated
        };

        private USTHumanoid[] _randomUSTs = new USTHumanoid[]
        {
            USTHumanoid.VictoryGesture,
            USTHumanoid.WipeForehead,
            USTHumanoid.JumpScared,
            USTHumanoid.DismissingGesture
        };

        private FSHumanoid[] _randomExpressions = new FSHumanoid[]
        {
            FSHumanoid.ASSERTIVE,
            FSHumanoid.AVERSE,
            FSHumanoid.INTERESTED,
            FSHumanoid.SAD,
            FSHumanoid.SATISFIED,
        };

        private Vector3 _goal;

        private Stay _stay;
        private Locomotion _locomotion;
        private Face _face;
        private LookSimple _lookSimple;
    }
}
