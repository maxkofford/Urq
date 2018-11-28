namespace Urq.Testing
{

    using UnityEngine;
    using UnityEditor;
    using UnityEngine.TestTools;
    using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;

    public class BattleTest
    {
        private Battle CreateBattleObject()
        {
            GameObject go = new GameObject();
            return go.AddComponent<Battle>();        
        }

        [Test]
        public void BattleTest1()
        {
            Battle b = CreateBattleObject();
            b.AddTeam(InputManagerHelper.ControllerType.ComputerEasy);

            List<BattleTeam> teams = b.GetTeams();
            Assert.AreEqual(teams.Count, 1);
            Assert.AreEqual(teams[0].MyControllerType, InputManagerHelper.ControllerType.ComputerEasy);
        }

        [Test]
        public void BattleTest2()
        {
            Battle b = CreateBattleObject();
            b.AddTeam(InputManagerHelper.ControllerType.ComputerEasy);
            b.AddTeam(InputManagerHelper.ControllerType.ComputerEasy);
            b.AddTeam(InputManagerHelper.ControllerType.ComputerEasy);

            List<BattleTeam> teams = b.GetTeams();
            Assert.AreEqual(teams.Count, 3);
            Assert.AreEqual(teams[0].MyControllerType, InputManagerHelper.ControllerType.ComputerEasy);
            Assert.AreEqual(teams[1].MyControllerType, InputManagerHelper.ControllerType.ComputerEasy);
            Assert.AreEqual(teams[2].MyControllerType, InputManagerHelper.ControllerType.ComputerEasy);
        }

        [Test]
        public void BattleTest3()
        {
            Battle b = CreateBattleObject();
            b.AddTeam(InputManagerHelper.ControllerType.ComputerEasy);
            b.AddTeam(InputManagerHelper.ControllerType.ComputerHard);
            b.AddTeam(InputManagerHelper.ControllerType.ComputerNormal);
            b.AddTeam(InputManagerHelper.ControllerType.LocalPlayer1);
            b.AddTeam(InputManagerHelper.ControllerType.LocalPlayer2);

            List<BattleTeam> teams = b.GetTeams();
            Assert.AreEqual(teams.Count, 5);
            Assert.AreEqual(teams[0].MyControllerType, InputManagerHelper.ControllerType.ComputerEasy);
            Assert.AreEqual(teams[1].MyControllerType, InputManagerHelper.ControllerType.ComputerHard);
            Assert.AreEqual(teams[2].MyControllerType, InputManagerHelper.ControllerType.ComputerNormal);
            Assert.AreEqual(teams[3].MyControllerType, InputManagerHelper.ControllerType.LocalPlayer1);
            Assert.AreEqual(teams[4].MyControllerType, InputManagerHelper.ControllerType.LocalPlayer2);
        }
    }
}