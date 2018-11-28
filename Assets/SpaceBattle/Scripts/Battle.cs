namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents one battl3e 
    /// </summary>
    public class Battle : MonoBehaviour
    {
        /// <summary>
        /// All the teams for this battle
        /// </summary>
        private List<BattleTeam> teams = new List<BattleTeam>();

        /// <summary>
        /// Gets all the current teams
        /// </summary>
        /// <returns></returns>
        public List<BattleTeam> GetTeams()
        {
            return teams;
        }

        /// <summary>
        /// Adds a team to the battle
        /// </summary>
        public BattleTeam AddTeam(InputManagerHelper.ControllerType type)
        {
            BattleTeam newTeam = new BattleTeam(this, type);
            teams.Add(newTeam);
            return newTeam;
        }

        /// <summary>
        /// Adds a prefab to a team
        /// Instantiates the prefab before adding it to the team
        /// </summary>
        public void AddShipPrefabToTeam(Ship ship, BattleTeam targetTeam)
        {
            if (!teams.Contains(targetTeam))
            {
                Debug.LogError("Cant add a ship to a team when its team doesnt exist!");
            }

            targetTeam.AddShipToTeam(ship);
        }    

        /// <summary>
        /// Adds a ship prefab thats already on a team to the current inbattle
        /// </summary>
        public void AddShipToBattle(Ship ship, BattleTeam targetTeam)
        {
            if (!teams.Contains(targetTeam))
            {
                Debug.LogError("Cant add a ship to a battle when its team doesnt exist!");
            }

            targetTeam.AddShipToInBattle(ship);
        }

        /// <summary>
        /// Adds a ship thats inbattle to the dead ships
        /// </summary>
        public void AddShipToDeadShips(Ship ship, BattleTeam targetTeam)
        {
            if (!teams.Contains(targetTeam))
            {
                Debug.LogError("Cant add a ship to dead when its team doesnt exist!");
            }

            targetTeam.AddShipToDeadShips(ship);
        }

        /// <summary>
        /// Ends the battle
        /// </summary>
        public void EndBattle()
        {
            foreach (BattleTeam team in teams)
            {
                team.CleanUp();
            }
        }
    }
}
