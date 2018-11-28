namespace Urq
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents one team in a battle
    /// Pipeline is AddShipToTeam -> AddShipToInBattle -> AddShipsToDeadShips
    /// </summary>
    public class BattleTeam
    {
        public delegate void BattleTeamLoseEvent(BattleTeam losingTeam);

        public event BattleTeamLoseEvent LoseEvent;    

        public Battle MyBattle
        {
            get;
            private set;
        }

        public InputManagerHelper.ControllerType MyControllerType
        {
            get;
            private set;
        }

        private List<Ship> AliveOutOfBattleShips = new List<Ship>();
        private List<Ship> InBattleShips = new List<Ship>();
        private List<Ship> DeadShips = new List<Ship>();
        private HashSet<BattleTeam> allies = new HashSet<BattleTeam>();
       
        public BattleTeam(Battle battle, InputManagerHelper.ControllerType controller)
        {
            MyBattle = battle;
            MyControllerType = controller;
            allies.Add(this);     
        }

        public void AddAlly(BattleTeam other)
        {
            allies.Add(other);
        }

        public List<Ship> GetAliveOutOfBattleShips()
        {
            return AliveOutOfBattleShips;
        }

        public List<Ship> GetInBattleShips()
        {
            return InBattleShips;
        }

        public List<Ship> GetDeadShips()
        {
            return DeadShips;
        }

        public void AddShipToTeam(Ship s)
        {
            AliveOutOfBattleShips.Add(s);
        }

        public void AddShipToInBattle(Ship s)
        {
            if (AliveOutOfBattleShips.Contains(s))
            {
                AliveOutOfBattleShips.Remove(s);
            }
            else
            {
                Debug.LogError("Trying to move a ship to InBattle but its not registered for the battle!");
                return;
            }

            InBattleShips.Add(s);

            Poolable newPooledItem = PoolManager.GetAPooled(s.MyPoolable);
            newPooledItem.transform.SetParent(MyBattle.transform);
            newPooledItem.gameObject.SetActive(true);        
            Ship newShip = newPooledItem.GetComponent<Ship>();
            
            newShip.Init(this);
            newShip.DeathEvent += ShipDeathEvent;
        }

        public void AddShipToDeadShips(Ship s)
        {
            if (InBattleShips.Contains(s))
            {
                InBattleShips.Remove(s);
            }
            else
            {
                Debug.LogError("Trying to move a ship to InBattle but its not registered for the battle!");
                return;
            }

            DeadShips.Add(s);
            if (InBattleShips.Count == 0 && AliveOutOfBattleShips.Count == 0)
            {
                LoseEvent.Invoke(this);
            }
        }

        /// <summary>
        /// All the stuff needed to clean up a battle team (when a battle ends)
        /// </summary>
        public void CleanUp()
        {
            foreach (Ship s in InBattleShips)
            {
                s.MyPoolable.ReturnMe();
            }
        }

        /// <summary>
        /// A list of all ships that are still alive for this team
        /// </summary>
        public List<Ship> GetAllAliveShips()
        {
            List<Ship> allAlives = new List<Ship>();
            allAlives.AddRange(AliveOutOfBattleShips);
            allAlives.AddRange(InBattleShips);
            return allAlives;
        }

        /// <summary>
        /// Whether or not this team is allied with another (true if allied false otherwise)
        /// Will also say true for itself
        /// </summary>
        public bool IsAlliedWith(BattleTeam other)
        {
            if (allies.Contains(other))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// A ship announcing that it has died
        /// </summary>
        /// <param name="targetShip"></param>
        private void ShipDeathEvent(Ship targetShip)
        {
            AddShipToDeadShips(targetShip);
            targetShip.MyPoolable.ReturnMe();
        }  


    }
}