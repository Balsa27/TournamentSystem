using BLL.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Entities.TournamentSystems;
using Modules.Tools;
using UnitTesting.Manager_test.MockDatabaseClasses;
using Match = Modules.Entities.Match;

namespace UnitTesting.Manager_test.Tests
{
    [TestClass]
    public class ScheduleManagerTests
    {
        ScheduleManager scheduleManager;
        TournamentManager tournamentManager;
        UserManager userManager;
        TournamentRepositoryMock tournamentRepository;
        UserRepositoryMock userRepositoryMock;
        private Customer fisrtPlayer = new Customer("Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private Customer forthPlayer = new Customer("Balsa4", "Balsa", "Balsa", "Balsa", "balsasaa@gmail", Gender.MALE);
        private Customer thirdPlayer = new Customer("Balsa3as", "Balsa", "Balsa", "Balsa", "balssagasgga@gmail", Gender.MALE);
        private List<Customer> players = new List<Customer>();

        public ScheduleManagerTests()
        {
            scheduleManager = new ScheduleManager(new ScheduleRepositoryMock());
            tournamentRepository = new TournamentRepositoryMock();
            userRepositoryMock = new UserRepositoryMock();
            userManager = new UserManager(userRepositoryMock);
            tournamentManager = new TournamentManager(tournamentRepository, userManager, scheduleManager);
        }

        [TestMethod]
        public void ScheduleManagerConstructorTest()
        {
            ScheduleRepositoryMock scheduleRepository = new ScheduleRepositoryMock();
            scheduleManager = new ScheduleManager(scheduleRepository);
            Assert.IsNotNull(scheduleManager);
        }

        [TestMethod]
        public void SetPlayerScoresTest()
        {
            Tournament tournament = new Tournament
            (Guid.NewGuid(), "string.Empty", "string.Empty", 2, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);
            tournament.RegisterPlayer(fisrtPlayer);
            tournament.RegisterPlayer(secondPlayer);
            tournament.RegisterPlayer(thirdPlayer);
            tournament.RegisterPlayer(forthPlayer);
            tournament.StartTournamnet();
            List<Match> matches = tournament.GetAllMatches();
            scheduleManager.SetPlayerScores(tournament, matches[0], 21, 3  );
        }

        [TestMethod]
        public void SetPlayerScoresTest2()
        {
            Tournament tournament = new Tournament
            (Guid.NewGuid(), "string.Empty", "string.Empty", 2, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);
            tournament.RegisterPlayer(fisrtPlayer);
            tournament.RegisterPlayer(secondPlayer);
            tournament.RegisterPlayer(thirdPlayer);
            tournament.RegisterPlayer(forthPlayer);
            tournament.StartTournamnet();
            List<Match> matches = tournament.GetAllMatches();
            scheduleManager.SetPlayerScores(tournament, matches[1], 21, 3);
        }
        
        [TestMethod]
        public void SetPlayerScoresWithAnIrrelevantMatchTest()
        {
            Tournament tournament = new Tournament
            (Guid.NewGuid(), "string.Empty", "string.Empty", 2, 15,
                Gender.FEMALE, new DateTime(2017, 1, 1),
                new DateTime(2018, 1, 1), new RoundRobin(), TournamentStatus.CREATED, players);
            tournament.RegisterPlayer(fisrtPlayer);
            tournament.RegisterPlayer(secondPlayer);
            tournament.RegisterPlayer(thirdPlayer);
            tournament.RegisterPlayer(forthPlayer);
            tournament.StartTournamnet();
            List<Match> matches = tournament.GetAllMatches();
            Assert.ThrowsException<ArgumentOutOfRangeException> (() => scheduleManager.SetPlayerScores(tournament, matches[7], 21, 3));
        }
    }
}
