// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using VirtualBeings.Tech.Beings.Humanoid;

namespace VirtualBeings.Beings.Humanoid
{
    public class FTTHumanoid : FTTHumanoidBase
    {
        public static new FTTHumanoid Default = new(0, nameof(Default), true);

        public static FTTHumanoid WithFOT = new(nameof(WithFOT));
        public static FTTHumanoid Rage = new(nameof(Rage));
        public static FTTHumanoid Laugh = new(nameof(Laugh));
        public static FTTHumanoid Exclamation = new(nameof(Exclamation));

        private FTTHumanoid(string name) : base(name) { }
        private FTTHumanoid(int id, string name, bool setNone) : base(id, name, setNone) { }
    }
}
