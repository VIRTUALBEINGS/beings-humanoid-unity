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

        private FTTHumanoid(string name) : base(name) { }
        private FTTHumanoid(int id, string name, bool setNone) : base(id, name, setNone) { }
    }
}
