// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections;
using UnityEngine;
using VirtualBeings.BehaviorComposition;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid
{
    [Serializable]
    public class IdleSettings { }

    public class Idle : HumanoidRootActivity<Idle>
    {
        public override ExecutionType ExecutionType { get; protected set; } = ExecutionType.Default;

        public IdleSettings IdleSettings { get; }

        public Idle(IHumanoidActivity parent, IdleSettings idleSettings) : base(parent, null)
        {
            IdleSettings = idleSettings;
            OnCalculatePriority = () => Priority_7_Idle;
           
            Initialize += () =>
            {
                _goal = new Vector3(0f, 0f, 0f);
                _locomotion = new Locomotion(this);
                _lookSimple = new LookSimple(this, targetIInteractable: null, false, false, 0.7f, null);
                _face = new Face(this);
            };
        }

        protected override IEnumerator MainProcess()
        {
            _locomotion.Start(BodyResetType.ResetToDefault);
            _lookSimple.Start();
            _face.Start();

            while (FOREVER)
            {
                float a = Rand.Range(0f, 360);
                _goal = Vector3.zero + new Vector3(Mathf.Sin(a), 0f, Mathf.Cos(a)) * Rand.Range(2f, 10f);

                FSHumanoid randomExpression = _randomExpressions[Rand.Range(0, _randomExpressions.Length)];
                _face.DoExpression(randomExpression, Rand.Range(2f, 4f), Rand.Range(0.5f, 1f));

                _locomotion.MoveTo(RSHumanoid.Walk, () => _goal);
                yield return new SuspendWhile(() => _locomotion.IsMoving());

                yield return new SuspendForDuration(Rand.Range(0.5f, 1f));

                if(Rand.Bool)
                {
                    STHumanoid randomST = _randomSTs[Rand.Range(0, _randomSTs.Length)];
                    _locomotion.RequestST(randomST, transitionLeftRight: Rand.sign); // Some ST ignore left / right
                }
                else
                {
                    USTHumanoid randomUST = _randomUSTs[Rand.Range(0, _randomUSTs.Length)];
                    _locomotion.DoUST(randomUST, transitionLeftRight: Rand.sign); // Some UST ignore left / right
                }

                yield return new SuspendWhile(() => _locomotion.IsInTransition());
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
        //private MoveTo _moveTo;
        private Locomotion _locomotion;
        private Face _face;
        private LookSimple _lookSimple;
    }
}
