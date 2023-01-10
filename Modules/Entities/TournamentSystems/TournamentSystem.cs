using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Entities.TournamentSystems
{
    public abstract class TournamentSystem 
    {
        public List<Round> Rounds { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public Round CurrentRound { get; protected set; }


        public TournamentSystem()
        {
            Rounds = new List<Round>();
        }

        public abstract List<Round> GetAllAvailableRounds();

        public abstract List<Round> ComputeAllAvailableRounds(List<Customer> players);

    }
}
