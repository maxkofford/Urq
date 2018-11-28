namespace Urq
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Urq/Attacks/AttackDataBacicSpawnShot")]
    public class AttackDataBasicSpawnShot : IAttackData
    {
        /// <summary>
        /// The shot to spawn
        /// </summary>
        public IShot shotPrefabToSpawn;

        /// <summary>
        /// How many seconds to wait inbetween doing one shot
        /// </summary>
        public float fireRate;

        /// <summary>
        /// The button to do a shot on
        /// </summary>
        public InputManagerHelper.Buttons buttonToShootOn;

        /// <summary>
        /// Gets the attack for this particular data
        /// </summary>
        public override IAttack GetAttack()
        {
            return new AttackBasicSpawnShot(this);
        }
    }

    /// <summary>
    /// Spawns a shot at the ship that can do whatever it wants
    /// </summary>
    public class AttackBasicSpawnShot : IAttack
    {

        public AttackBasicSpawnShot(AttackDataBasicSpawnShot myData)
        {
            MyData = myData;
        }

        public AttackDataBasicSpawnShot MyData
        {
            get;
            private set;
        }

        /// <summary>
        /// Runtime Cooldown for shots/abilities
        /// </summary>
        private float timeOfLastShot = -100;

        /// <summary>
        /// Spawn a attack bullet every so often
        /// </summary>    
        public override void ApplyAttack(Ship myShip, InputManagerHelper.ControlInputs inputs, Rigidbody2D myBody)
        {
            float currentTime = Time.time;
            if (timeOfLastShot + MyData.fireRate < currentTime)
            {

                if (inputs.GetButtonValue(MyData.buttonToShootOn) != 0)
                {
                    Poolable newPooledItem = PoolManager.GetAPooled(MyData.shotPrefabToSpawn.GetComponent<Poolable>());
                    newPooledItem.transform.position = myShip.transform.position;
                    newPooledItem.transform.rotation = myShip.transform.rotation;

                    IShot newShot = newPooledItem.GetComponent<IShot>();
                    newShot.Init(myShip);
                    newShot.ShotFinished += ShotFinished;
                    myShip.AddShot(newShot);

                    newPooledItem.gameObject.SetActive(true);
                    timeOfLastShot = currentTime;
                }
            }
        }

        private void ShotFinished(IShot myself, Ship myShip)
        {
            myShip.RemoveShot(myself);
            myself.DeInit();
            myself.ShotFinished -= ShotFinished;
            PoolManager.ReturnAPooled(myself.GetComponent<Poolable>());
        }
    }
}