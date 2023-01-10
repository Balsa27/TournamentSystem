using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;

namespace Modules.Interfaces.Repository
{
    public interface ITournamentRepository
    {
        public List<Tournament> GetAllTournaments();
        public void CreateTournament(Tournament tournament);
        public void DeleteTournament(Tournament tournament);
        public void UpdateTournament(Tournament tournament);
        public void RegisterPlayer(Customer customer, Tournament tournament);
        public void UnRegisterPlayer(Customer customer, Tournament tournament);
        public void StartTournament(Tournament tournament);
    }
}
