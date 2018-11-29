namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    //using UnityStandardAssets.CrossPlatformInput;

    /// <summary>
    /// For controlling a ship
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class ShipControl : MonoBehaviour , InputManagerHelper.IInputRequester
    {
        private Rigidbody2D myRigidBody;

        private InitialShipStats myInitialShipStats;

        private bool isInitialized = false;

        private List<IAttack> attacks = new List<IAttack>();

        /// <summary>
        /// This should be a constructor parameter but monobhaviour >.<
        /// </summary>
        public Ship MyShip
        {
            get;
            private set;
        }

        public void Initialize(Ship s)
        {
            if (isInitialized)
            {
                Debug.LogError("Trying to initialize twice!");
                return;
            }
            
            
            MyShip = s;

            if (myRigidBody == null)
            {
                myRigidBody = this.GetComponent<Rigidbody2D>();
            }

            myInitialShipStats = s.GetInitialShipStats();
            myRigidBody.mass =  myInitialShipStats.Mass;
            myRigidBody.angularDrag = myInitialShipStats.AngularDrag;
            myRigidBody.drag = myInitialShipStats.Drag;

            foreach (var attackData in myInitialShipStats.Attacks)
            {
                attacks.Add(attackData.GetAttack());
            }

            isInitialized = true;
        }

        public InputManagerHelper.ControlInputs GetInputs()
        {
            return InputManagerHelper.GetCurrentControlInputs(MyShip.MyControllerType , this);
        }

        private void FixedUpdate()
        {
            if (isInitialized)
            {
                InputManagerHelper.ControlInputs currentInputs = GetInputs();

                myInitialShipStats.Movement.ApplyMovement(currentInputs, myRigidBody);
                foreach (IAttack oneAttack in attacks)
                {
                    oneAttack.ApplyAttack(MyShip, currentInputs, myRigidBody);
                }
            }
        }
    } 
}