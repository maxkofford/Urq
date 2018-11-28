namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(Battle))]
    public class BattleEditor : Editor
    {
        Battle myTarget;

        private List<bool> teamsFoldout = new List<bool>();
        private List<List<bool>> shipsFoldout = new List<List<bool>>();

        private Ship targetShipPrefab;
        private InputManagerHelper.ControllerType controllerType;
        void OnEnable()
        {
            myTarget = (Battle)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (Application.isPlaying)
            {   
                targetShipPrefab = (Ship)EditorGUILayout.ObjectField("Target to add", targetShipPrefab, typeof(Ship), true);
                controllerType = (InputManagerHelper.ControllerType)EditorGUILayout.EnumPopup("Controller: ", controllerType);                            
               
                if (GUILayout.Button("Add team"))
                {
                    myTarget.AddTeam(controllerType);
                    
                }

                List<BattleTeam> teams = myTarget.GetTeams();
                int spot = 0;
                foreach (BattleTeam team in teams)
                {
                    if (teamsFoldout.Count < teams.Count)
                    {
                        teamsFoldout.Add(false);
                    }

                    teamsFoldout[spot] = EditorGUILayout.Foldout(teamsFoldout[spot], "Team " + spot + " (" + team.MyControllerType + ")");

                    if (teamsFoldout[spot])
                    {

                        GUILayout.Label("Alives: ");
                        EditorGUI.indentLevel++;
                        List<Ship> alives = team.GetAliveOutOfBattleShips();
                        foreach (Ship s in alives)
                        {
                            EditorGUILayout.ObjectField("Ship: ", s, typeof(Ship), false);
                        }
                        EditorGUI.indentLevel--;

                        GUILayout.Label("InBattles: ");
                        EditorGUI.indentLevel++;
                        List<Ship> inbattles = team.GetInBattleShips();
                        foreach (Ship s in inbattles)
                        {
                            EditorGUILayout.ObjectField("Ship: ", s, typeof(Ship), false);
                        }
                        EditorGUI.indentLevel--;

                        GUILayout.Label("Deads: ");
                        EditorGUI.indentLevel++;
                        List<Ship> deads = team.GetDeadShips();
                        foreach (Ship s in deads)
                        {
                            EditorGUILayout.ObjectField("Ship: ", s, typeof(Ship), false);
                        }
                        EditorGUI.indentLevel--;

                        if (targetShipPrefab != null)
                        {
                            if (GUILayout.Button("Add to team"))
                            {
                                myTarget.AddShipPrefabToTeam(targetShipPrefab, team);
                            }

                            if (GUILayout.Button("Add to inbattle"))
                            {
                                myTarget.AddShipToBattle(targetShipPrefab, team);
                            }

                            if (GUILayout.Button("Add to dead"))
                            {
                                myTarget.AddShipToDeadShips(targetShipPrefab, team);
                            }
                        }
                    }
                    spot++;
                }




                //toggles[i] = EditorGUILayout.Foldout(toggles[i], manager.AllPersistentSettings[i].name);

                //if (toggles[i])
                {
                }



                //toggles[i] = EditorGUILayout.Foldout(toggles[i], manager.AllPersistentSettings[i].name);
            }
        }
    }
}