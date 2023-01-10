using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Interfaces.Repository;

namespace UnitTesting.Manager_test.MockDatabaseClasses
{
    public class TournamentRepositoryMock : ITournamentRepository
    {
        public List<Tournament> GetAllTournaments()
        {
            return new List<Tournament>();
        }

        public void CreateTournament(Tournament tournament)
        {
        }

        public void DeleteTournament(Tournament tournament)
        {
        }

        public void UpdateTournament(Tournament tournament)
        {
        }

        public void RegisterPlayer(Customer customer, Tournament tournament)
        {
        }

        public void UnRegisterPlayer(Customer customer, Tournament tournament)
        {
        }

        public void StartTournament(Tournament tournament) //testmeeee
        {
            throw new NotImplementedException();
        }
    }
}
