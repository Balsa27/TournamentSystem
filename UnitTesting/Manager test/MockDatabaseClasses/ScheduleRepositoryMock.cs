using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using Modules.Entities;
using Modules.Interfaces;

namespace UnitTesting.Manager_test.MockDatabaseClasses
{
    internal class ScheduleRepositoryMock : IScheduleRepository
    {
        public List<Round> GetAllTournamentRounds(Tournament tournament)
        {
            return new List<Round>();
        }

        public void SaveMatchResults(Match match, int firstPlayerScore, int secondPlayerScore)
        {
        }

        public List<Round> GetAllTournamentRounds(Guid tournamentId)
        {
            throw new NotImplementedException();
        }

        public void SaveTournamentRounds(Tournament tournament)
        {
        }
    }
}
