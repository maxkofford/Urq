namespace Urq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A shot that just moves forward for a set amount of time
    /// </summary>
    public class ShotBasicStraight : IShot
    {
        public float LifeSpan;
        public int Damage;
        public float speed;

        private float startingTime;
        private Transform myTransform;

        public override void Init(Ship myShip)
        {
            base.Init(myShip);
            startingTime = Time.time;
            myTransform = this.transform;
        }

        public override void DeInit()
        {
            startingTime = Time.time;
            base.DeInit();   
        }

        public void FixedUpdate()
        {
            float currentTime = Time.time;
            if (currentTime > startingTime + LifeSpan)
            {
                CleanMyselfUp();
            }
            else
            {
                myTransform.localPosition += myTransform.up * speed;
            }
        }

        public override void ApplyOther(GameObject other)
        {
            //Probably need to apply damage here
            CleanMyselfUp();
        }

        public void CleanMyselfUp()
        {       
            InvokeShotFinished();
        }
    }
}