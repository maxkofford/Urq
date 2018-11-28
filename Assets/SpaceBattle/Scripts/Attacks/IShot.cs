namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class IShot : MonoBehaviour
    {
        private bool HasInit = false;

        public delegate void ShotFinishEvent( IShot myself, Ship myShip);

        public event ShotFinishEvent ShotFinished;

        public Ship MyShip
        {
            get;
            private set;
        }

        public virtual void Init(Ship myShip)
        {
            if (!HasInit)
            {             
                MyShip = myShip;
                HasInit = true;
            }
            else
            {
                Utilities.Debug.LogError("Trying to init a shot twice!");
            }
        }

        

        public virtual void DeInit()
        {
            if (HasInit)
            {
                MyShip = null;
                HasInit = false;
            }
            else
            {
                Utilities.Debug.LogError("Trying to DeInit a shot that hasnt been inited!");
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            //checking if we hit our own ship (so we should ignore)
            if (collision.otherRigidbody != null)
            {
                ApplyOther(collision.otherRigidbody.gameObject);
            }
        }

        public abstract void ApplyOther(GameObject other);

        public void InvokeShotFinished()
        {
            ShotFinished.Invoke(this, MyShip);
        }
    }
}