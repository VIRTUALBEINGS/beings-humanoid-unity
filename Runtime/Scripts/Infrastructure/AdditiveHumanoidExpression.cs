// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

public class AdditiveHumanoidExpression : AdditiveHumanoidExpressionBase
{
    // ----------------------------
    // Additive expressions
    //

    public static new AdditiveHumanoidExpression None { get; private set; } =
        new((int)IAdditiveExpressionEnums.None, nameof(None), true);

    // ---------------------------
    // Static interface
    //

    public static new AdditiveHumanoidExpression FromID(int id) =>
        (AdditiveHumanoidExpression)AdditiveHumanoidExpressionBase.FromID(id);

    public static new AdditiveHumanoidExpression FromName(string name) =>
        (AdditiveHumanoidExpression)AdditiveHumanoidExpressionBase.FromName(name);

    // ---------------------------
    // Private stuff
    //

    private AdditiveHumanoidExpression(string name)
        : base(name)
    {
    }

    private AdditiveHumanoidExpression(int id, string name, bool setNone)
        : base(id, name, setNone)
    {
    }
}
