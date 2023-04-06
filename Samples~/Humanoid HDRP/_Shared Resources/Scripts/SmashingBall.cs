// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System.Collections;
using UnityEngine;

namespace VirtualBeings.Demo
{
    public class SmashingBall : MonoBehaviour
    {
        [SerializeField]
        private float _initialDelay = 3f;

        [SerializeField]
        private float _initialVelocity = 30f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_initialDelay);

            Rigidbody rb = GetComponent<Rigidbody>();

            rb.useGravity = true;
            rb.isKinematic = false;

            rb.AddForce(transform.TransformDirection(Vector3.forward) * _initialVelocity, ForceMode.VelocityChange);
        }
    }
}
