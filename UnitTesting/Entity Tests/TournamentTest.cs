using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Entities.TournamentSystems;
using Modules.Tools;

namespace UnitTesting.Tests
{
    [TestClass]
    public class TournamentTest
    {
        Customer fisrtPlayer = new Customer("Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        Customer thirdPlayer = new Customer("Balsa3", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        Customer forthPlayer = new Customer("Balsa4", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private List<Customer> players = new List<Customer>();


        private void RegisterOddNumberOfPlayerToTournament(Tournament tournament)
        {
            tournament.RegisterPlayer(fisrtPlayer);
            tournament.RegisterPlayer(secondPlayer);
            tournament.RegisterPlayer(thirdPlayer);
        }

        private void RegisterEvenNumberOfPlayerToTournament(Tournament tournament)
        {
            tournament.RegisterPlayer(fisrtPlayer);
            tournament.RegisterPlayer(secondPlayer);
            tournament.RegisterPlayer(thirdPlayer);
            tournament.RegisterPlayer(forthPlayer);
        }

        [TestMethod]
        public void GettersTest()
        {
            Tournament tournament = new Tournament
            (Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "string.Empty", "string.Empty", 10, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);

            Guid id = Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989");
            string title = "string.Empty";
            string location = "string.Empty";
            int minPlayers = 10;
            int maxPlayers = 15;
            Gender gender = Gender.FEMALE;
            DateTime startDate = new DateTime(2017, 1, 1);
            DateTime endDate = new DateTime(2018, 1, 1);
            TournamentSystem system = tournament.TournamentSystem;
            TournamentStatus status = TournamentStatus.CREATED;
            Assert.AreEqual(id, tournament.Id);
            Assert.AreEqual(title, tournament.Tittle);
            Assert.AreEqual(location, tournament.Location);
            Assert.AreEqual(minPlayers, tournament.MinPlayers);
            Assert.AreEqual(maxPlayers, tournament.MaxPlayers);
            Assert.AreEqual(startDate, tournament.StartDate);
            Assert.AreEqual(endDate, tournament.EndDate);
            Assert.AreEqual(system, tournament.TournamentSystem);
            Assert.AreEqual(status, tournament.Status);
        }

        [TestMethod]
        public void CreateTournamentWithoutId()
        {
            Tournament tournament = new Tournament
            ("string.Empty", "string.Empty", 10, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin());
            Assert.IsNotNull(tournament);
        }

        [TestMethod]
        public void CreateTournamentWithId()
        {
            Tournament tournament = new Tournament
            (new Guid(), "string.Empty", "string.Empty", 10, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);
            Assert.IsNotNull(tournament);
        }

        [TestMethod]
        public void CreateTournamentWithoutTitle()
        {
            Assert.ThrowsException<ArgumentException>(() => new Tournament
            (new Guid(), string.Empty, "Somewhere", 10, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players));
        }

        [TestMethod]
        public void CreateTournamentWithoutLocation()
        {
            Assert.ThrowsException<ArgumentException>(() => new Tournament
            (new Guid(), "string.Empty", string.Empty, 10, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players));
        }

        [TestMethod]
        public void CreateTournamentWithCorrectData()
        {
            Tournament tournament = new Tournament
                (new Guid(), "Tournament", "Somewhere", 2, 10,
                    Gender.FEMALE, new DateTime(2017, 1, 1),
                    new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);
            Assert.IsNotNull(tournament);
        }

        [TestMethod]
        public void CreateTournamentWithIncorrectDates()
        {
            Assert.ThrowsException<ArgumentException>(() => new Tournament
            (new Guid(), "Tournament", "Somewhere", 2, 10,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2016, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players));
        }

        [TestMethod]
        public void CreateTournamentWithIncorrectPlayerNumbers()
        {
            Assert.ThrowsException<ArgumentException>(() => new Tournament
            (new Guid(), "Tournament", "Somewhere", 15, 10,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players));
        }

        [TestMethod]
        public void CreateTournamentWithNegativeMinumumNumberOfPlayers()
        {
            Assert.ThrowsException<ArgumentException>(() => new Tournament
            (new Guid(), "Tournament", "Somewhere", -1, 10,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players));
        }

        [TestMethod]
        public void CreateTournamentWithNegativeMaximumNumberOfPlayers()
        {
            Assert.ThrowsException<ArgumentException>(() => new Tournament
            (new Guid(), "Tournament", "Somewhere", 2, -1,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players));
        }

        [TestMethod]
        public void PlayerRegistrationTest()
        {
            Tournament tournament = new Tournament
            (new Guid(), "Tournament", "Somewhere", 2, 10,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);

            RegisterOddNumberOfPlayerToTournament(tournament);
            Assert.AreEqual(3, tournament.RegisteredPlayers.Count);
        }

        [TestMethod]
        public void StartTournamentThatIsAlreadyStarted()
        {
            Tournament tournament = new Tournament
            (new Guid(), "Tournament", "Somewhere", 2, 10,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.STARTED, players);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                tournament.StartTournamnet();
            });
        }

        [TestMethod]
        public void StartRoundRobinTournamentWithoutMinumunRequiredPlayers()
        {
            Tournament tournament = new Tournament
            ("Tournament", "Somewhere", 5, 10,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin());
            RegisterOddNumberOfPlayerToTournament(tournament);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                tournament.StartTournamnet();
            });
        }

        [TestMethod]
        public void StartTournamentTest()
        {
            Tournament tournament = new Tournament
            (Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "string.Empty", "string.Empty", 2, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);
            tournament.RegisteredPlayers.Add(fisrtPlayer);
            tournament.RegisteredPlayers.Add(secondPlayer);
            tournament.RegisteredPlayers.Add(forthPlayer);
            tournament.StartTournamnet();
            Assert.IsTrue(tournament.Status == TournamentStatus.STARTED);
            Assert.IsTrue(tournament.AllTournamentRounds is not null);
        }

        [TestMethod]
        public void StartStartedTournamentTest()
        {
            Tournament tournament = new Tournament
            (Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "string.Empty", "string.Empty", 2, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.FINISHED, players);
            tournament.RegisteredPlayers.Add(fisrtPlayer);
            tournament.RegisteredPlayers.Add(secondPlayer);
            tournament.RegisteredPlayers.Add(forthPlayer);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                tournament.StartTournamnet();
            });
        }

        [TestMethod]
        public void StartTournamentWithNotEnoughPlayersTest()
        {
            Tournament tournament = new Tournament
            (Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "string.Empty", "string.Empty", 5, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.STARTED, players);
            tournament.RegisteredPlayers.Add(fisrtPlayer);
            tournament.RegisteredPlayers.Add(secondPlayer);
            tournament.RegisteredPlayers.Add(forthPlayer);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                tournament.StartTournamnet();
            });
        }


        [TestMethod]
        public void SetRegisteredPlayersTest()
        {
            Tournament tournament = new Tournament
            (Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "string.Empty", "string.Empty", 5, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.STARTED, this.players);
            players.Add(fisrtPlayer);
            players.Add(secondPlayer);
            players.Add(thirdPlayer);
            Assert.AreEqual(3, tournament.RegisteredPlayers.Count);
        }

        [TestMethod]
        public void UnRegisterPlayerTest()
        {
            Tournament tournament = new Tournament
            (Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "string.Empty", "string.Empty", 5, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.STARTED, players);
            tournament.RegisteredPlayers.Add(fisrtPlayer);
            Assert.IsNotNull(tournament.RegisteredPlayers);
            tournament.UnRegisterPlayer(fisrtPlayer);
            Assert.IsTrue(tournament.RegisteredPlayers.Count == 0);
        }

        [TestMethod]
        public void GetAllMatchesTest()
        {
            Tournament tournament = new Tournament
            (Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "string.Empty", "string.Empty", 2, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);
            tournament.RegisteredPlayers.Add(fisrtPlayer);
            tournament.RegisteredPlayers.Add(secondPlayer);
            tournament.RegisteredPlayers.Add(thirdPlayer);
            tournament.StartTournamnet();
            List<Match> allMATCHES = tournament.GetAllMatches();
            Assert.IsNotNull(allMATCHES);
        }

    }
}
