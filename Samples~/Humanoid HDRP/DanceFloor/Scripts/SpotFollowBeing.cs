// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;
using VirtualBeings.Tech.UnityIntegration;

namespace VirtualBeings.Beings.Humanoid.Samples.DanceSample
{
    public class SpotFollowBeing : MonoBehaviour
    {
        public int BeingID;
        private Being BeingToFollow;

        Container Container => Container.Instance;

        // Update is called once per frame
        void Update()
        {
            if (BeingToFollow == null)
            {
                BeingToFollow = Container.BeingManager.Beings.FirstOrDefault(b => b.BeingID == BeingID);
            }

            if (BeingToFollow != null)
            {
                transform.LookAt(BeingToFollow.transform);
            }
        }
    }
}
