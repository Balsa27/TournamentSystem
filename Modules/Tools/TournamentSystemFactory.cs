using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Entities.TournamentSystems;

namespace Modules.Tools
{
    public static class TournamentSystemFactory
    {
        private static IDictionary<string, TournamentSystem> TournamentSystems;

        public static TournamentSystem GetTournamentSystem(string name)
        {
            if (TournamentSystems is null)
            {
                TournamentSystems = new Dictionary<string, TournamentSystem>();
                TournamentSystems.Add("Round Robin", new RoundRobin());
                TournamentSystems.Add("Double Round Robin", new DoubleRoundRobin());
            }
            return TournamentSystems[name];
        }

    }
}
