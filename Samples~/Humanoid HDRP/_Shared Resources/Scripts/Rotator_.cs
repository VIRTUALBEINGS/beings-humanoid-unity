// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEngine;

namespace VirtualBeings.Demo
{
    [DisallowMultipleComponent]
    public class Rotator_ : MonoBehaviour
    {
        public enum RotationAxis { X, Y, Z };
        public RotationAxis rotationAxis;
        public float angularSpeed;
        private Vector3 axis;

        void Start()
        {
            switch (rotationAxis)
            {
                case RotationAxis.X: axis = Vector3.right; break;
                case RotationAxis.Y: axis = Vector3.up; break;
                case RotationAxis.Z: axis = Vector3.forward; break;
            }
        }

        void Update()
        {
            transform.Rotate(axis, Time.deltaTime * angularSpeed);
        }
    }
}
