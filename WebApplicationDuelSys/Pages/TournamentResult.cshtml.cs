using BLL.Managers;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modules.Entities;
using Modules.Interfaces.Repository;

namespace WebApplicationDuelSys.Pages
{
    public class TournamentResultModel : PageModel
    {
        ScheduleManager scheduleManager;
        TournamentManager tournamentManager;
        Tournament Tournament;
        public List<Round> AllTournamentRounds { get; set; }
        
        public TournamentResultModel(ScheduleManager _scheduleManager, TournamentManager _tournamentManager)
        {
            scheduleManager = _scheduleManager;
            tournamentManager = _tournamentManager;
        }

        public void OnGet(Guid tournamentId)
        {
            Tournament = tournamentManager.GetTournamentById(tournamentId);
            AllTournamentRounds = scheduleManager.GetAllAvailableRounds(Tournament);
        }
       
    }
}
