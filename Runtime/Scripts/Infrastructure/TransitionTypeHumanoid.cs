// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class TransitionTypeHumanoid : TransitionTypeHumanoidBase
    {
        public static new TransitionTypeHumanoid Default = new(0, nameof(Default), true);
        public static new TransitionTypeHumanoid DefaultLeft = new(
            (int)ITransitionTypeEnums.DefaultLeft,
            nameof(DefaultLeft),
            true
        );
        public static new TransitionTypeHumanoid DefaultRight = new(
            (int)ITransitionTypeEnums.DefaultRight,
            nameof(DefaultRight),
            true
        );
        public static new TransitionTypeHumanoid DefaultForward = new(
            (int)ITransitionTypeEnums.DefaultForward,
            nameof(DefaultForward),
            true
        );
        public static new TransitionTypeHumanoid DefaultBackwardLeft = new(
            (int)ITransitionTypeEnums.DefaultBackwardLeft,
            nameof(DefaultBackwardLeft),
            true
        );
        public static new TransitionTypeHumanoid DefaultBackwardRight = new(
            (int)ITransitionTypeEnums.DefaultBackwardRight,
            nameof(DefaultBackwardRight),
            true
        );
        public static new TransitionTypeHumanoid NiceStop = new(
            (int)ITransitionTypeEnums.NiceStop,
            nameof(NiceStop),
            true
        );
        public static new TransitionTypeHumanoid NoImpulse = new(
            (int)ITransitionTypeEnums.NoImpulse,
            nameof(NoImpulse),
            true
        );

        public static TransitionTypeHumanoid Surprised = new(nameof(Surprised));

        public static new TransitionTypeHumanoid FromID(int id) =>
            (TransitionTypeHumanoid)TransitionTypeHumanoidBase.FromID(id);

        public static new TransitionTypeHumanoid FromName(string name) =>
            (TransitionTypeHumanoid)TransitionTypeHumanoidBase.FromName(name);

        private TransitionTypeHumanoid(string name) : base(name) { }
        private TransitionTypeHumanoid(int id, string name, bool setSpecial) : base(id, name, setSpecial) { }
    }
}
