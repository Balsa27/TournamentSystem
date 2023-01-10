using BLL.Managers;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modules.Entities;
using Modules.Interfaces.Repository;
using Modules.Tools;

namespace WebApplicationDuelSys.Pages
{
    public class TournamentHistoryModel : PageModel
    {
        private readonly TournamentManager _tournamentManager;
        private readonly ScheduleManager _scheduleManager;
        private List<Tournament> Tournaments { get; set; }
        private readonly UserManager _userManager;

        public TournamentHistoryModel(TournamentManager tournamentManager, ScheduleManager schedulerManager)
        {
            _tournamentManager = tournamentManager;
            _scheduleManager = schedulerManager;
            Tournaments = _tournamentManager.GetAllTournaments();
        }

        public List<Tournament> GetAllFinishedTournaments()
        {
            return Tournaments.Where(t => t.Status == TournamentStatus.FINISHED).ToList();
        }

        //public List<Round> AllTournamentRounds(Guid tournamentId)
        //{
        //    Tournament tournament = _tournamentManager.GetTournamentById(tournamentId);
        //    return _scheduleManager.GetAllAvailableRounds(tournament);
        //}
    }
}
