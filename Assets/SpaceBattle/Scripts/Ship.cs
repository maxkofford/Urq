namespace Urq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Utilities;

    /// <summary>
    /// Represents a ship to identify prefabs
    /// </summary>
    public class Ship : MonoBehaviour , IBattleTeamHolder 
    {
        public delegate void ShipEvent(Ship targetShip);

        public event ShipEvent DeathEvent;

        private ShipControl myControl;

        [SerializeField]
        private Poolable myPoolable;

        [SerializeField]
        private InitialShipStats myInitialShipStats;

        private bool hasInit = false;

        private HashSet<IShot> myShots = new HashSet<IShot>();

        public int MaximumHp;
        public int StartingHp;

        public int MaximumMana;
        public int StartingMana;

        public int CurrentHp
        {
            get;
            set;
        }
        public int CurrentMana
        {
            get;
            set;
        }

       

        public Poolable MyPoolable
        {
            get
            {
                return myPoolable;
            }
        }

        public ShipControl MyControl
        {
            get
            {
                return myControl;
            }
            set          
            {
                myControl = value;
                myControl.Initialize(this);
            }
        }    

        public BattleTeam MyTeam
        {
            get;
            private set;
        }

        public InputManagerHelper.ControllerType MyControllerType
        {
            get;
            private set;
        }

        public void Init(BattleTeam myTeam)
        {
            if (!hasInit)
            {
                MyTeam = myTeam;
                MyControllerType = MyTeam.MyControllerType;
                MyControl = this.gameObject.AddComponent<ShipControl>();
                CurrentHp = StartingHp;
                CurrentMana = StartingMana;
                hasInit = true;

            }
            else
            {
                Utilities.Debug.LogError("Trying to init twice");
            }
        }

        public InitialShipStats GetInitialShipStats()
        {
            return myInitialShipStats;
        }

        public HashSet<IShot> GetMyShots()
        {
            return myShots;
        }

        public void AddShot(IShot shot)
        {
            myShots.Add(shot);
        }

        public void RemoveShot(IShot shot)
        {
            myShots.Remove(shot);
        }

        public void Dead()
        {
            DeathEvent.Invoke(this);           
        }

        public BattleTeam GetMyBattleTeam()
        {
            return MyTeam;
        }
    }
}