namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Urq/ShipStats")]
    public class InitialShipStats : ScriptableObject
    {
        /// <summary>
        /// The movement to use for this ship
        /// </summary>
        [Tooltip("The movement to use for this ship")]
        [SerializeField]
        private IMovement movement;

        /// <summary>
        /// The attacks to use for this ship
        /// </summary>
        [Tooltip("The attacks to use for this ship")]
        [SerializeField]
        private List<IAttackData> attacks;

        /// <summary>
        /// The ships mass
        /// </summary>
        [Tooltip("The ships mass")]
        [SerializeField]
        private float mass = 1;

        /// <summary>
        /// The ships drag (friction)
        /// </summary>
        [Tooltip("The ships drag (friction)")]
        [SerializeField]
        private float drag = 0;

        /// <summary>
        /// The ships rotational drag
        /// </summary>
        [Tooltip("The ships rotational drag")]
        [SerializeField]
        private float angularDrag = .4f;

        public IMovement Movement
        {
            get
            {
                return movement;
            }
        }

        public List<IAttackData> Attacks
        {
            get
            {
                return attacks;
            }
        }

        public float Mass
        {
            get
            {
                return mass;
            }
        }

        public float Drag
        {
            get
            {
                return drag;
            }
        }

        public float AngularDrag
        {
            get
            {
                return angularDrag;
            }
        }
    }
}