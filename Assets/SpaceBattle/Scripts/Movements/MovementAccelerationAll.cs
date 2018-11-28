namespace Urq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents a movement type using acceleration for both thrust and rotation
    /// </summary>
    [CreateAssetMenu(menuName = "Urq/Movement/AccelerationAll")]
    public class MovementAccelerationAll : IMovement
    {
        /// <summary>
        /// The ships movement amount (speed for set speed and acceleration for set acceleration)
        /// </summary>
        [Tooltip("The ships movement amount (speed for set speed and acceleration for set acceleration)")]
        [SerializeField]
        private float acceleration = 1;

        /// <summary>
        /// The ships maximum speed
        /// </summary>
        [Tooltip("The ships maximum speed")]
        [SerializeField]
        private float maxSpeed = 1;

        /// <summary>
        /// The ships rotational acceleration
        /// </summary>
        [Tooltip("The ships rotational acceleration")]
        [SerializeField]
        private float rotationalAcceleration = 1;

        /// <summary>
        /// The ships maximum rotational speed
        /// </summary>
        [Tooltip("")]
        [SerializeField]
        private float maxRotationalSpeed = 1;

        public override void ApplyMovement(InputManagerHelper.ControlInputs inputs, Rigidbody2D myBody)
        {
            Transform targetTransform = myBody.transform;
            float verticalInput = inputs.VerticalInput;
            float horizontalInput = inputs.HorizontalInput;

            if (verticalInput > 0)
            {
                myBody.AddForce(targetTransform.up * verticalInput * acceleration);
            }

            myBody.AddTorque(-1 * horizontalInput * rotationalAcceleration);

            if (myBody.velocity.magnitude > maxSpeed)
            {
                myBody.velocity = myBody.velocity.normalized * maxSpeed;
            }

            if (Mathf.Abs(myBody.angularVelocity) > maxRotationalSpeed)
            {
                myBody.angularVelocity = Mathf.Sign(myBody.angularVelocity) * maxRotationalSpeed;
            }
        }
    }
}