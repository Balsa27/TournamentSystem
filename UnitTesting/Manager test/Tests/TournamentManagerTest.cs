using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Entities.TournamentSystems;
using Modules.Exceptions;
using Modules.Tools;
using Org.BouncyCastle.Security;
using UnitTesting.Manager_test.MockDatabaseClasses;

namespace UnitTesting.Manager_test.Tests
{
    [TestClass]
    public class TournamentManagerTest
    {
        private readonly UserManager _userManager;
        private readonly TournamentManager _tournamentManager;
        private readonly List<Tournament> allTournaments;
        private readonly List<Customer> allCustomers;
        Tournament tournament = new Tournament
        (new Guid(), "string.Empty", "string.Empty", 10, 15,
            Gender.FEMALE, new DateTime(2023, 1, 1),
            new DateTime(2025, 1, 1), new RoundRobin(), TournamentStatus.CREATED, new List<Customer>());
        private Customer secondPlayer = new Customer(Guid.NewGuid(), "Balsa2", "Balsa", "Balssfa", "Balsa", "balsa@gmail", Gender.MALE);
        private Customer fisrtPlayer = new Customer(Guid.NewGuid(), "Balsa", "Balsa", "Balsaasf", "Balsa", "balsaaa@gmail", Gender.MALE);
        private Customer forthPlayer = new Customer(Guid.NewGuid(), "Balsa4", "Balsa", "Balsaas", "Balsa", "balsasaa@gmail", Gender.MALE);
        private Customer thirdPlayer = new Customer(Guid.NewGuid(), "Balsa3as", "Balsa", "Balsaa", "Balsa", "balssagasgga@gmail", Gender.MALE);
        

        public TournamentManagerTest()
        {
            _userManager = new UserManager(new UserRepositoryMock());
            _tournamentManager = new TournamentManager(new TournamentRepositoryMock(), _userManager, null);
            allTournaments = _tournamentManager.GetAllTournaments();
            allCustomers = _userManager.GetAllCustomers();
        }

        [TestMethod]
        public void GetAllTournamentsEmptyTest()
        {
            Assert.AreEqual(allTournaments.Count, 0);
        }

        [TestMethod]
        public void GetAllTournamentsTest()
        {
            allTournaments.Add(tournament);
            Assert.AreEqual(allTournaments.Count, 1);
        }

        [TestMethod]
        public void GetAllTournamentsNotEmptyTest()
        {
            List<Tournament> expectedTournaments = new List<Tournament>();
            expectedTournaments.Add(tournament);
            allTournaments.Add(tournament);
            CollectionAssert.AreEquivalent(allTournaments, expectedTournaments);
        }

        [TestMethod]
        public void CreateTournamentTest()
        {
            Tournament newTournament = new Tournament(new Guid(), "string.Empty", "string.Empty", 10, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, new List<Customer>());
            _tournamentManager.CreateTournament(newTournament);
            
            List<Tournament> expectedTournaments = new List<Tournament>();
            expectedTournaments.Add(newTournament);
           
            Assert.IsNotNull(allTournaments);
            CollectionAssert.AreEquivalent(expectedTournaments, _tournamentManager.GetAllTournaments());
        }

        [TestMethod]
        public void CreateTournamentWithSameTittleTest()
        {
            Tournament newTournament = new Tournament(new Guid(), "string.Empty", "string.Empty", 10, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, new List<Customer>());
            _tournamentManager.CreateTournament(newTournament);
            Assert.ThrowsException<DuplicateEntryException>(() =>
            {
                _tournamentManager.CreateTournament(new Tournament(new Guid(), "string.Empty", "string.Empty", 10, 15,
                    Gender.FEMALE, new DateTime(2017, 1, 1),
                    new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, new List<Customer>()));
            });
        }

        [TestMethod]
        public void UpdateTournamentTest()
        {
            _tournamentManager.CreateTournament(tournament);
            _tournamentManager.UpdateTournament(tournament);
            Assert.AreEqual(tournament, _tournamentManager.GetAllTournaments()[0]);
        }

        [TestMethod]
        public void DeleteTournamentTest()
        {
            _tournamentManager.CreateTournament(tournament);
            Assert.IsNotNull(_tournamentManager.GetAllTournaments());

            _tournamentManager.DeleteTournament(tournament);
            Assert.IsTrue(_tournamentManager.GetAllTournaments().Count == 0);
        }

        [TestMethod]
        public void TryRegisterPlayerTest()
        {
            _userManager.CreateCustomer(secondPlayer);
            _tournamentManager.CreateTournament(tournament);
            _tournamentManager.TryRegisterPlayerToTournament(secondPlayer.Id, tournament.Id);
            Assert.AreEqual(tournament.RegisteredPlayers.Count, 1);
        }

        [TestMethod]
        public void TryRegisterPlayerTwiceTest()
        {
            _userManager.CreateCustomer(secondPlayer);
            _tournamentManager.CreateTournament(tournament);
            _tournamentManager.TryRegisterPlayerToTournament(secondPlayer.Id, tournament.Id);
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _tournamentManager.TryRegisterPlayerToTournament(secondPlayer.Id, tournament.Id);
            });
        }

        [TestMethod]
        public void TryRegisterPlayerSevenDaysBeforeATournamentTest()
        {
            Tournament tournamentt = new Tournament
            (new Guid(), "string.Empty", "string.Empty", 10, 15,
                Gender.FEMALE, new DateTime(2022, 6, 3),
                new DateTime(2022, 6, 15), new RoundRobin(), TournamentStatus.CREATED, allCustomers);
            _userManager.CreateCustomer(secondPlayer);
            _tournamentManager.CreateTournament(tournamentt);
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _tournamentManager.TryRegisterPlayerToTournament(secondPlayer.Id, tournamentt.Id);
            });
        }

        [TestMethod]
        public void TryRegisterPlayerPlayerAtAFullCapacityTournamentTest()
        {
            Tournament tournamentt = new Tournament
            (Guid.NewGuid(), "string.Empty", "string.Empty", 2, 3,
                Gender.FEMALE, new DateTime(2025, 6, 3),
                new DateTime(2026, 6, 15), new RoundRobin(), TournamentStatus.CREATED, allCustomers);
            _userManager.CreateCustomer(fisrtPlayer);
            _userManager.CreateCustomer(secondPlayer);
            _userManager.CreateCustomer(thirdPlayer);
            _userManager.CreateCustomer(forthPlayer);
            _tournamentManager.CreateTournament(tournamentt);
            _tournamentManager.TryRegisterPlayerToTournament(secondPlayer.Id, tournamentt.Id);
            _tournamentManager.TryRegisterPlayerToTournament(fisrtPlayer.Id, tournamentt.Id);
            _tournamentManager.TryRegisterPlayerToTournament(forthPlayer.Id, tournamentt.Id);
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _tournamentManager.TryRegisterPlayerToTournament(thirdPlayer.Id, tournamentt.Id);
            });
        }

        [TestMethod]
        public void TryRegisterPlayerWhoDoesntExist()
        {
            Tournament tournamentt = new Tournament
            (new Guid(), "string.Empty", "string.Empty", 2, 3,
                Gender.FEMALE, new DateTime(2025, 6, 3),
                new DateTime(2026, 6, 15), new RoundRobin(), TournamentStatus.CREATED, allCustomers);
            _tournamentManager.CreateTournament(tournamentt);
            Assert.IsFalse(_tournamentManager.TryRegisterPlayerToTournament(thirdPlayer.Id, tournamentt.Id));
        }

        [TestMethod]
        public void TryRegisterPlayerToTournamentThatDoesntExist()
        {
            Tournament tournamentt = new Tournament
            (new Guid(), "string.Empty", "string.Empty", 2, 3,
                Gender.FEMALE, new DateTime(2025, 6, 3),
                new DateTime(2026, 6, 15), new RoundRobin(), TournamentStatus.CREATED, allCustomers);
            _userManager.CreateCustomer(thirdPlayer);
            Assert.IsFalse(_tournamentManager.TryRegisterPlayerToTournament(thirdPlayer.Id, tournamentt.Id));
        }

        [TestMethod]
        public void TryUnRegisterPlayerFromTournament()
        {
            TryRegisterPlayerTest();
            _tournamentManager.TryUnRegisterPlayerToTournament(secondPlayer.Id, tournament.Id);
            Assert.AreEqual(tournament.RegisteredPlayers.Count, 0);
            Assert.IsTrue(tournament.RegisteredPlayers.Count == 0);
        }

        [TestMethod]
        public void TryUnRegisterPlayerThatIsNotInTheTournamentTest()
        {
            TryRegisterPlayerTest();
            _tournamentManager.TryUnRegisterPlayerToTournament(secondPlayer.Id, tournament.Id);
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _tournamentManager.TryUnRegisterPlayerToTournament(secondPlayer.Id, tournament.Id);

            });
        }

        [TestMethod]
        public void TryUnRegisterPlayerToATournamentThatDoesntExist()
        {
            Tournament tournamentt = new Tournament
            (new Guid(), "string.Empty", "string.Empty", 2, 3,
                Gender.FEMALE, new DateTime(2025, 6, 3),
                new DateTime(2026, 6, 15), new RoundRobin(), TournamentStatus.CREATED, allCustomers);
            _userManager.CreateCustomer(thirdPlayer);
            Assert.IsFalse(_tournamentManager.TryUnRegisterPlayerToTournament(thirdPlayer.Id, tournamentt.Id));
        }

        [TestMethod]
        public void TryUnRegisterPlayerThatDoesntExist()
        {
            Tournament tournamentt = new Tournament
            (new Guid(), "string.Empty", "string.Empty", 2, 3,
                Gender.FEMALE, new DateTime(2025, 6, 3),
                new DateTime(2026, 6, 15), new RoundRobin(), TournamentStatus.CREATED, allCustomers);
            _tournamentManager.CreateTournament(tournamentt);
               Assert.IsFalse( _tournamentManager.TryUnRegisterPlayerToTournament(thirdPlayer.Id, tournamentt.Id));

        }

        [TestMethod]
        public void GetTournamentByIdTest()
        {
            _tournamentManager.CreateTournament(tournament);
            Tournament t = _tournamentManager.GetTournamentById(tournament.Id);
            Assert.AreEqual(t, _tournamentManager.GetAllTournaments()[0]);
        }

        [TestMethod]
        public void StartTournamentTest()
        {
            Tournament tournamentt = new Tournament
            (new Guid(), "string.Empty", "string.Empty", 2, 15,
                Gender.FEMALE, new DateTime(2023, 1, 1),
                new DateTime(2025, 1, 1), new RoundRobin(), TournamentStatus.CREATED, allCustomers);
            _tournamentManager.CreateTournament(tournamentt);
            tournamentt.RegisterPlayer(fisrtPlayer);
            tournamentt.RegisterPlayer(secondPlayer);
            tournamentt.RegisterPlayer(thirdPlayer);
            tournamentt.RegisterPlayer(forthPlayer);
            tournamentt.StartTournamnet();
            Assert.IsTrue(tournamentt.Status == TournamentStatus.STARTED);
            Assert.IsTrue(tournamentt.AllTournamentRounds is not null);
        }
    }
}
