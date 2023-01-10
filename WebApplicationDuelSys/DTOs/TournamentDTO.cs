using System.ComponentModel.DataAnnotations;
using Modules.Entities;
using Modules.Entities.TournamentSystems;

namespace WebApplicationDuelSys.DTOs
{
    public class TournamentDTO
    {
        [Required]
        public string Tittle { get; set; }

        [Required]
        public int MaxPlayers { get; set; }
       
        [Required]
        public int MinPlayers { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public List<Customer> RegisteredPlayers { get; set; }

        [Required]
        public TournamentSystem TournamentSystem { get; set; }

    }
}
