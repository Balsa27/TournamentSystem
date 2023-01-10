using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Entities
{
    public class Round
    {
        public List<Match> Matches { get; private set; }
       
        public Guid Id { get; private set; }

        public int Position { get; set; }

        public Round(List<Match> matches)
        {
            Matches = matches;
            Id = Guid.NewGuid();
        }

        public Round(List<Match> matches, Guid id)
        {
            Matches = matches;
            Id = id;
        }
        
        public override string ToString() => $"Round {Position}";
           
    }
}
