namespace Urq.Testing
{

    using UnityEngine;
    using UnityEditor;
    using UnityEngine.TestTools;
    using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;

    public class BattleTeamTest
    {
        private Ship CreateShipObject()
        {
            GameObject go = new GameObject();
            return go.AddComponent<Ship>();            
        }

        [Test]
        public void BattleTeamTest1()
        {
            BattleTeam bt = new BattleTeam(null, InputManagerHelper.ControllerType.ComputerEasy);
            Assert.AreEqual(InputManagerHelper.ControllerType.ComputerEasy, bt.MyControllerType);
            
            Ship s = CreateShipObject();
            bt.AddShipToTeam(s);
            List<Ship> outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(1, outships.Count);
            Assert.AreEqual(s, outships[0]);
        }

        [Test]
        public void BattleTeamTest2()
        {
            BattleTeam bt = new BattleTeam(null, InputManagerHelper.ControllerType.ComputerEasy);
            Ship s = CreateShipObject();
            bt.AddShipToTeam(s);
            bt.AddShipToTeam(s);

            List<Ship> outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(2, outships.Count);
            Assert.AreEqual(s, outships[0]);
            Assert.AreEqual(s, outships[1]);
        }

        [Test]
        public void BattleTeamTest3()
        {
            BattleTeam bt = new BattleTeam(null, InputManagerHelper.ControllerType.ComputerEasy);
            Ship s = CreateShipObject();
            Ship s2 = CreateShipObject();
            bt.AddShipToTeam(s);
            bt.AddShipToTeam(s2);

            List<Ship> outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(2, outships.Count);
            Assert.AreEqual(s, outships[0]);
            Assert.AreEqual(s2, outships[1]);
        }

        [Test]
        public void BattleTeamTest4()
        {
            BattleTeam bt = new BattleTeam(null, InputManagerHelper.ControllerType.ComputerEasy);
            Ship s = CreateShipObject();
            Ship s2 = CreateShipObject();
            bt.AddShipToTeam(s);
            bt.AddShipToTeam(s2);

            List<Ship> outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(2, outships.Count);
            Assert.AreEqual(true, outships.Contains(s));
            Assert.AreEqual(true, outships.Contains(s2));

            bt.AddShipToInBattle(s);

            outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(1, outships.Count);
            Assert.AreEqual(true, outships.Contains(s2));

            List<Ship> inships = bt.GetInBattleShips();
            Assert.AreEqual(1, inships.Count);
            Assert.AreEqual(true, inships.Contains(s));

            bt.AddShipToInBattle(s2);

            outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(0, outships.Count);
            Assert.AreEqual(false, outships.Contains(s2));

            inships = bt.GetInBattleShips();
            Assert.AreEqual(2, inships.Count);
            Assert.AreEqual(true, inships.Contains(s));
            Assert.AreEqual(true, inships.Contains(s2));

        }

        [Test]
        public void BattleTeamTest5()
        {
            BattleTeam bt = new BattleTeam(null, InputManagerHelper.ControllerType.ComputerEasy);
            Ship s = CreateShipObject();
            Ship s2 = CreateShipObject();
            bt.AddShipToTeam(s);
            bt.AddShipToTeam(s2);

            List<Ship> outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(2, outships.Count);
            Assert.AreEqual(true, outships.Contains(s));
            Assert.AreEqual(true, outships.Contains(s2));

            bt.AddShipToInBattle(s);

            outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(1, outships.Count);
            Assert.AreEqual(true, outships.Contains(s2));

            List<Ship> inships = bt.GetInBattleShips();
            Assert.AreEqual(1, inships.Count);
            Assert.AreEqual(true, inships.Contains(s));

            bt.AddShipToInBattle(s2);

            outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(0, outships.Count);
            Assert.AreEqual(false, outships.Contains(s2));

            inships = bt.GetInBattleShips();
            Assert.AreEqual(2, inships.Count);
            Assert.AreEqual(true, inships.Contains(s));
            Assert.AreEqual(true, inships.Contains(s2));

            bt.AddShipToDeadShips(s);

            outships = bt.GetAliveOutOfBattleShips();
            Assert.AreEqual(0, outships.Count);
            Assert.AreEqual(false, outships.Contains(s2));

            inships = bt.GetInBattleShips();
            Assert.AreEqual(1, inships.Count);
            Assert.AreEqual(false, inships.Contains(s));
            Assert.AreEqual(true, inships.Contains(s2));

            List<Ship> deadships = bt.GetDeadShips();
            Assert.AreEqual(1, deadships.Count);
            Assert.AreEqual(true, deadships.Contains(s));
            Assert.AreEqual(false, deadships.Contains(s2));


        }
    }
}
