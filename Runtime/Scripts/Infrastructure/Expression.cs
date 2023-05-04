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

        public static ExpressionWrapper NEUTRAL(FTTHumanoid ftt, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.None, ftt, intensity01, randomization01);
        }
        public static ExpressionWrapper NEUTRAL(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.None, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper NEUTRAL(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.None, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper SHY(FTTHumanoid ftt, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SHY, ftt, intensity01, randomization01);
        }
        public static ExpressionWrapper SHY(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SHY, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper SHY(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SHY, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper SATISFIED(FTTHumanoid ftt, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SATISFIED, ftt, intensity01, randomization01);
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

        public static ExpressionWrapper INTERESTED(FTTHumanoid ftt, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.INTERESTED, ftt, intensity01, randomization01);
        }
        public static ExpressionWrapper INTERESTED(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.INTERESTED, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper INTERESTED(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.INTERESTED, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper AVERSE(FTTHumanoid ftt, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.AVERSE, ftt, intensity01, randomization01);
        }
        public static ExpressionWrapper AVERSE(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.AVERSE, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper AVERSE(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.AVERSE, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper SAD(FTTHumanoid ftt, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SAD, ftt, intensity01, randomization01);
        }
        public static ExpressionWrapper SAD(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SAD, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper SAD(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SAD, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper ASSERTIVE(FTTHumanoid ftt, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.ASSERTIVE, ftt, intensity01, randomization01);
        }
        public static ExpressionWrapper ASSERTIVE(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.ASSERTIVE, FTTHumanoid.Default, intensity01, randomization01);
        }
        public static ExpressionWrapper ASSERTIVE(FSHumanoid secondary, float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.ASSERTIVE, FTTHumanoid.Default, secondary, intensity01, randomization01);
        }

        public static ExpressionWrapper Confused(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.Confused, FTTHumanoid.Default, intensity01, randomization01);
        }

        public static ExpressionWrapper PuffedCheeks(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.PuffedCheeks, FTTHumanoid.Default, intensity01, randomization01);
        }

        public static ExpressionWrapper Grimace(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.Grimace, FTTHumanoid.Default, intensity01, randomization01);
        }

        public static ExpressionWrapper EyesSquinted(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.EyesSquinted, FTTHumanoid.Default, intensity01, randomization01);
        }

        public static ExpressionWrapper EyebrowsRaise(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.EyebrowsRaise, FTTHumanoid.Default, intensity01, randomization01);
        }

        public static ExpressionWrapper SeriousLook(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SeriousLook, FTTHumanoid.Default, intensity01, randomization01);
        }

        public static ExpressionWrapper SmileSubtle(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SmileSubtle, FTTHumanoid.Default, intensity01, randomization01);
        }

        public static ExpressionWrapper SmileWide(float intensity01 = 1f, float randomization01 = 1f)
        {
            return new(FSHumanoid.SmileWide, FTTHumanoid.Default, intensity01, randomization01);
        }
    }
}
