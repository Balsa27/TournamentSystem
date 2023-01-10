using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Tools;

namespace Modules.Entities.TournamentSystems
{
    public class RoundRobin : TournamentSystem
    {
        public RoundRobin() : base()
        {
            Name = "Round Robin";
            Description = "OVERSOUL";
        }

        private int currentRoundIndex = -1; 
        private List<Customer> Players;
        private Customer PlayerDummy = new Customer("NO PLAYER", "string.Empty", "string.Empty", "string.Empty", "string.Empty", Gender.OTHER);

        public override List<Round> ComputeAllAvailableRounds(List<Customer> players) //bugs out with a dummy 
        {
            Players = players;
            
            if (players.Count % 2 == 1)
            {
                Players.Add(PlayerDummy);
            }
            int roundCount = players.Count - 1; //numb or players - 1

            for (int i = 0; i < roundCount; i++)
            {
                Round round = CreateRound(Players);
                Rounds.Add(round); //adding the matches to the round

                //player rotation
                Players = RotatePlayers(Players); //rotating the players
            }

            if (Rounds.Count > 0)
            {
                CurrentRound = Rounds[0];
                currentRoundIndex = 0;
            }
            return Rounds;
            
        }

        public List<Customer> RotatePlayers(List<Customer> players)
        {
            Customer lastPlayer = players[players.Count - 1]; //last player
            for (int z = players.Count - 1; z > 1; z--)
            {
                players[z] = players[z - 1]; //rotating the players
            }
            players[1] = lastPlayer; //putting the last player in the first position
            return players;
        }

        public Round CreateRound(List<Customer> players)
        {
            List<Match> matches = new List<Match>();

            for (int j = 0; j < players.Count / 2; j++) //splitting the player list into two groups
            {
                if (players[j] == PlayerDummy || players[players.Count - j - 1] == PlayerDummy)
                {
                    continue; //if condition is done it skips to the next execution of the for loop
                }
                matches.Add(new Match(0, 0, players[j], players[players.Count - j - 1])); //creating a match with the first player in the first group and the last player in the second group
            }

            return new Round(matches);
        }

        public override List<Round> GetAllAvailableRounds()
        {
            return Rounds;
        }
       
    }
}
