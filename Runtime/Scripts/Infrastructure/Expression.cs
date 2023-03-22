// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;

namespace VirtualBeings.Beings.Humanoid
{
    public static class Expression
    {
        public static ExpressionWrapper None() => new(FSHumanoid.None, FTTHumanoid.Default, 0f, 0f);

        public static ExpressionWrapper NEUTRAL(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.None, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper NEUTRAL(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.None, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper SHY(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SHY, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper SHY(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SHY, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper SATISFIED(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SATISFIED, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper SATISFIED(
            FSHumanoid secondary,
            float intensity01 = 1f,
            float randomization01 = 1f
        )
        {
            return new(FSHumanoid.SATISFIED, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper INTERESTED(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.INTERESTED, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper INTERESTED(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.INTERESTED, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper AVERSE(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.AVERSE, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper AVERSE(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.AVERSE, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper SAD(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SAD, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper SAD(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SAD, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper ASSERTIVE(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.ASSERTIVE, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper ASSERTIVE(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.ASSERTIVE, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }
    }
}
