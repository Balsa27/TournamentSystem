using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Entities.TournamentSystems;
using Modules.Tools;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace UnitTesting.Tests
{
    [TestClass]
    public class RoundRobinTest
    {
        private readonly Customer fisrtPlayer = new Customer("Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly Customer thirdPlayer = new Customer("Balsa3", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly Customer forthPlayer = new Customer("Balsa4", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private readonly List<Customer> players = new List<Customer>();
        private readonly List<Customer> expectedPlayers = new List<Customer>();
        RoundRobin roundRobin = new RoundRobin();        

        private void AddEvenNumberOfPlayersToList()
        {
            players.Add(fisrtPlayer);
            players.Add(secondPlayer);
            players.Add(thirdPlayer);
            players.Add(forthPlayer);
        }

        private void AddEvenNumberOfExpectedPlayersToList()
        {
            expectedPlayers.Add(fisrtPlayer);
            expectedPlayers.Add(forthPlayer);
            expectedPlayers.Add(secondPlayer);
            expectedPlayers.Add(thirdPlayer);
        }

        private void AddOddNumberOfPlayersToList()
        {
            players.Add(fisrtPlayer);
            players.Add(secondPlayer);
            players.Add(thirdPlayer);
        }

        private void AddOddNumberOfExpectedPlayersToList()
        {
            expectedPlayers.Add(fisrtPlayer);
            expectedPlayers.Add(thirdPlayer);
            expectedPlayers.Add(secondPlayer);
        }

        [TestMethod]
        public void ConstrouctorTest()
        {
            roundRobin = new RoundRobin();
            Assert.IsNotNull(roundRobin);
        }
        

        [TestMethod]
        public void EvenPlayerRotationTest()
        {
            AddEvenNumberOfPlayersToList();
            List<Customer> rotatedPlayers = roundRobin.RotatePlayers(players);
            AddEvenNumberOfExpectedPlayersToList();
            CollectionAssert.AreEquivalent(expectedPlayers, rotatedPlayers);
        }

        [TestMethod]
        public void OddPlayerRotationTest()
        {
            AddOddNumberOfPlayersToList();
            List<Customer> rotatedPlayers = roundRobin.RotatePlayers(players);
            AddOddNumberOfExpectedPlayersToList();
            CollectionAssert.AreEquivalent(expectedPlayers, rotatedPlayers);
        }

        [TestMethod]
        public void CreateRoundForEvenNumberOfPlayers()
        {
            AddEvenNumberOfPlayersToList();
            Round round = roundRobin.CreateRound(players);
            Assert.AreEqual(2, round.Matches.Count);
            Assert.AreEqual(fisrtPlayer, round.Matches[0].FirstPlayer);
            Assert.AreEqual(forthPlayer, round.Matches[0].SecondPlayer);
            Assert.AreEqual(secondPlayer, round.Matches[1].FirstPlayer);
            Assert.AreEqual(thirdPlayer, round.Matches[1].SecondPlayer);
        }

        [TestMethod]
        public void CreateRoundForOddNumberOfPlayers()
        {
            AddOddNumberOfPlayersToList();
            Round round = roundRobin.CreateRound(players);

            Console.WriteLine(round.Matches[0].FirstPlayer.FirstName);
            Console.WriteLine(round.Matches[0].SecondPlayer.FirstName);
            Assert.AreEqual(1, round.Matches.Count);
            Assert.AreEqual(fisrtPlayer, round.Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, round.Matches[0].SecondPlayer);
        }

        [TestMethod]
        public void ComputeAllAvailableRoundsWithEvenNumberOfPlayers()
        {
            AddEvenNumberOfPlayersToList();
            List<Round> rounds = roundRobin.ComputeAllAvailableRounds(players);

            Assert.AreEqual(3, rounds.Count);
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
        }

        [TestMethod]
        public void ComputeAllAvailableRoundsWithOddNumberOfPlayers()
        {
            AddOddNumberOfPlayersToList();
            List<Round> rounds = roundRobin.ComputeAllAvailableRounds(players);
            Assert.AreEqual(3, rounds.Count);
            Assert.AreEqual(1, rounds[0].Matches.Count);
            Assert.AreEqual(secondPlayer, rounds[0].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[0].Matches[0].SecondPlayer);
            Assert.AreEqual(1, rounds[1].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[1].Matches[0].FirstPlayer);
            Assert.AreEqual(thirdPlayer, rounds[1].Matches[0].SecondPlayer);
            Assert.AreEqual(1, rounds[2].Matches.Count);
            Assert.AreEqual(fisrtPlayer, rounds[2].Matches[0].FirstPlayer);
            Assert.AreEqual(secondPlayer, rounds[2].Matches[0].SecondPlayer);
        }

        [TestMethod]
        public void GetAllAvailableRoundsTest()
        {
            roundRobin = new RoundRobin();
            AddEvenNumberOfPlayersToList();
            roundRobin.ComputeAllAvailableRounds(players);
            List<Round> rounds = roundRobin.GetAllAvailableRounds();
            Assert.IsNotNull(rounds);
            Assert.AreEqual(3, rounds.Count);
        }
    }
}

