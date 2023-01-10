using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Entities.TournamentSystems;
using Modules.Tools;

namespace UnitTesting.Entity_Tests
{
    [TestClass]
    public class DoubleRoundRobinTest
    {
        private readonly Customer fisrtPlayer = new Customer("Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly Customer thirdPlayer = new Customer("Balsa3", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly Customer forthPlayer = new Customer("Balsa4", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly List<Customer> players = new List<Customer>();
        private readonly List<Customer> expectedPlayers = new List<Customer>();
        DoubleRoundRobin doubleRoundRobin= new DoubleRoundRobin();

        private void AddEvenNumberOfPlayersToList()
        {
            players.Add(fisrtPlayer);
            players.Add(secondPlayer);
            players.Add(thirdPlayer);
            players.Add(forthPlayer);
        }

        private void AddOddNumberOfPlayersToList()
        {
            players.Add(fisrtPlayer);
            players.Add(secondPlayer);
            players.Add(thirdPlayer);
        }

        [TestMethod]
        public void DoubleRoundRobinConstructorTest()
        {
            DoubleRoundRobin drr = new DoubleRoundRobin();
            Assert.IsNotNull(drr);
        }

        [TestMethod]
        public void ComputeAllAvailableRoundsWithEvenNumberOfPlayersTest()
        {
            AddEvenNumberOfPlayersToList();
            List<Round> rounds = doubleRoundRobin.ComputeAllAvailableRounds(players);

            Assert.AreEqual(6, rounds.Count);
            
            Assert.AreEqual(2, rounds[0].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[0].Matches[0].FirstPlayer);
            Assert.AreEqual(forthPlayer, rounds[0].Matches[0].SecondPlayer);
            Assert.AreEqual(secondPlayer, rounds[0].Matches[1].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[0].Matches[1].SecondPlayer);
            
            Assert.AreEqual(2, rounds[1].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[1].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[1].Matches[0].SecondPlayer);
            Assert.AreEqual(forthPlayer, rounds[1].Matches[1].FirstPlayer);
            Assert.AreEqual(secondPlayer, rounds[1].Matches[1].SecondPlayer);
            
            Assert.AreEqual(2, rounds[2].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[2].Matches[0].FirstPlayer);
            Assert.AreEqual(secondPlayer, rounds[2].Matches[0].SecondPlayer);
            Assert.AreEqual(thirdPlayer, rounds[2].Matches[1].FirstPlayer);
            Assert.AreEqual(forthPlayer, rounds[2].Matches[1].SecondPlayer);

            Assert.AreEqual(2, rounds[3].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[3].Matches[0].FirstPlayer);
            Assert.AreEqual(forthPlayer, rounds[3].Matches[0].SecondPlayer);
            Assert.AreEqual(secondPlayer, rounds[3].Matches[1].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[3].Matches[1].SecondPlayer);

            Assert.AreEqual(2, rounds[4].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[4].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[4].Matches[0].SecondPlayer);
            Assert.AreEqual(forthPlayer, rounds[4].Matches[1].FirstPlayer);
            Assert.AreEqual(secondPlayer, rounds[4].Matches[1].SecondPlayer);

            Assert.AreEqual(2, rounds[5].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[5].Matches[0].FirstPlayer);
            Assert.AreEqual(secondPlayer, rounds[5].Matches[0].SecondPlayer);
            Assert.AreEqual(thirdPlayer, rounds[5].Matches[1].FirstPlayer);
            Assert.AreEqual(forthPlayer, rounds[5].Matches[1].SecondPlayer);            
        }

        [TestMethod]
        public void ComputeAllAvailableRoundsWithOddNumberOfPlayers()
        {
            AddOddNumberOfPlayersToList();
            List<Round> rounds = doubleRoundRobin.ComputeAllAvailableRounds(players);
           
            Assert.AreEqual(6, rounds.Count);
            
            Assert.AreEqual(1, rounds[0].Matches.Count);
            Assert.AreEqual(secondPlayer, rounds[0].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[0].Matches[0].SecondPlayer);
            
            Assert.AreEqual(1, rounds[1].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[1].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[1].Matches[0].SecondPlayer);
            
            Assert.AreEqual(1, rounds[2].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[2].Matches[0].FirstPlayer);
            Assert.AreEqual(secondPlayer, rounds[2].Matches[0].SecondPlayer);

            Assert.AreEqual(1, rounds[3].Matches.Count);
            Assert.AreEqual(secondPlayer, rounds[3].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[3].Matches[0].SecondPlayer);

            Assert.AreEqual(1, rounds[4].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[4].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[4].Matches[0].SecondPlayer);

            Assert.AreEqual(1, rounds[5].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[5].Matches[0].FirstPlayer);
            Assert.AreEqual(secondPlayer, rounds[5].Matches[0].SecondPlayer);
        }

    }
}
