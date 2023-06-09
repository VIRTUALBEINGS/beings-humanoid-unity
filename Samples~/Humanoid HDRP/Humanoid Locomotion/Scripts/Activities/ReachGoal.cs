// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Samples.LocomotionSample
{
    [Serializable]
    public class ReachGoalSettings { }

    public class ReachGoal : HumanoidRootActivity<ReachGoal>
    {
        private Stay _stay;
        private Locomotion _locomotion;
        private ReachGoalSettings _settings;

        public ReachGoal(HumanoidMind parent, ReachGoalSettings settings) : base(parent, null)
        {
            _settings = settings;
            Initialize += () => {
                _locomotion = new(this);
                _stay       = new(this);
            };
        }

        protected override IEnumerator MainProcess()
        {
            float[] spawnDelaysByBeingID = new float[] { 4f, 0f, 2f };
            yield return new SuspendForDuration(spawnDelaysByBeingID[Being.BeingID]);

            _stay.Start();
            new ShowUST(this, _locomotion, 1.5f, 2f).Start();

            GoalInteractable goal = Container.Instance.InteractionDB.FindFirst(typeof(IInteractable), i => i is GoalInteractable) as GoalInteractable;

            if (goal != null)
            {
                yield return _locomotion.Start(RSHumanoid.Walk, () => goal.RootPosition);
            }

            while (FOREVER)
            {
                yield return null;
            }
        }

        private class ShowUST : HumanoidActivity<ShowUST>
        {
            private readonly Locomotion _locomotion;
            private readonly float _minDelayBetweenUSTs, _maxDelayBetweenUSTs;
            private readonly SuspendForDuration _suspend;

            public ShowUST(IHumanoidActivity parent, Locomotion locomotion, float minDelayBetweenUSTs, float maxDelayBetweenUSTs) : base(parent, null)
            {
                _locomotion = locomotion;
                _minDelayBetweenUSTs = minDelayBetweenUSTs;
                _maxDelayBetweenUSTs = maxDelayBetweenUSTs;
                _suspend = new SuspendForDuration();
            }

            protected override IEnumerator MainProcess()
            {
                while (FOREVER)
                {
                    yield return _suspend.Renew(_minDelayBetweenUSTs, _maxDelayBetweenUSTs);

                    PlayUST();
                }
            }

            private void PlayUST()
            {
                var usts = new USTHumanoid[]
                {
                    USTHumanoid.SwingArms_Subtle,
                    USTHumanoid.HeadTilt_Stretch,
                    USTHumanoid.TwistTorso_Subtle,
                    USTHumanoid.JumpScared,
                    USTHumanoid.DismissingGesture,
                    USTHumanoid.RollShoulder,
                    USTHumanoid.SaluteBriefRight,
                };

                _locomotion.DoUST(usts[Rand.Range(0, usts.Length)], transitionLeftRight: Rand.sign);
            }
        }
    }
}
