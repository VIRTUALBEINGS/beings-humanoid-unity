using System;
using System.Collections;
using UnityEngine;
using VirtualBeings.Beings.Humanoid;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.BehaviorComposition.Humanoids;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Humanoid.Samples.Dance
{
    [Serializable]
    public class DanceSettings
    {
    }

    public class Dance : HumanoidRootActivity<Dance>
    {
        public override ExecutionType ExecutionType { get; protected set; } = ExecutionType.Default;

        public Dance(IHumanoidActivity parent, DanceSettings settings) : base(parent, null)
        {
            Initialize += () => {
                _locomotion = new Locomotion(this);
                _lookDuringMoveTo = new LookDuringMoveTo(
                    this,
                    _locomotion,
                    emoter:
                    null,
                    minPitch: -5f,
                    maxPitch: 5f,
                    maxYaw: 45f,
                    averageDuration: 2f,
                    durationVariationFactor: 1.25f
                );

                _lookAround = new LookAround(this);
            };
        }

        private Locomotion _locomotion;
        private LookAround _lookAround;
        private LookDuringMoveTo _lookDuringMoveTo;

        protected override IEnumerator MainProcess()
        {
            float[] delays = { 1f, 2.5f, 4f };
            Vector3[] targets =
            {
                new(-1.5f, 0, 0.5f),
                new(1.8f, 0, 0.5f),
                new(-0.5f, 0, -1.0f),
            };

            RSHumanoid[] dances =
            {
                RSHumanoid.Dance_Samba, RSHumanoid.Dance_Robot, RSHumanoid.Dance_GangnamStyle,
            };

            ExpressionWrapper[] exprs =
                { Expression.SATISFIED(0.4f), Expression.NEUTRAL(), Expression.PUFFED_CHEEKS(0.5f), };

            int id = Being.BeingID;

            _lookAround.Start();

            SetExpression(exprs[id]);

            yield return new SuspendForDuration(delays[id]);

            _locomotion.Start(BodyResetType.ResetToDefault);
            _lookDuringMoveTo.Start();

            _locomotion.MoveTo(RSHumanoid.Walk, () => targets[id], allowAvoidance: false);
            yield return new SuspendWhile(_locomotion.IsMoving);

            _lookDuringMoveTo.Interrupt();

            _locomotion.SetTargetRS(dances[id], TransitionTypeHumanoid.Default);

            while (true)
            {
                yield return null;
            }
        }
    }
}
