using System.Linq.Expressions;
using BLL.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modules.Entities;
using Modules.Interfaces.Repository;
using Modules.Tools;

namespace WebApplicationDuelSys.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TournamentManager _tournamentManager;
        private readonly UserManager _userManager;
        public List<Tournament> Tournaments { get; set; }
        public Customer AuthenticatedCustomer { get; set; }

        public IndexModel(UserManager userManager, TournamentManager tournamentManager)
        {
            _userManager = userManager;
            _tournamentManager = tournamentManager;
            Tournaments = _tournamentManager.GetAllTournaments().Where(t => t.Status == TournamentStatus.CREATED).ToList();
        }

        public void OnGet()
        {
            if (User is not null && User.Identity.IsAuthenticated)
                AuthenticatedCustomer = _userManager.GetCustomerById(Guid.Parse(User.Identity.Name));
        }

        public IActionResult OnPostAuthentication()
        {
            return RedirectToPage("/Login");
        }
        
        public IActionResult OnPostAttendTournament(Guid tournamentId)
        {
            if (User is not null && User.Identity.IsAuthenticated)
                AuthenticatedCustomer = _userManager.GetCustomerById(Guid.Parse(User.Identity.Name));
            try
            {
                _tournamentManager.TryRegisterPlayerToTournament(AuthenticatedCustomer.Id, tournamentId);

            }
            catch (InvalidOperationException e)
            {
                TempData["Error"] = e.Message;
            }
            catch (ArgumentException e)
            {
                TempData["Error"] = e.Message;
            }

            return Page();
            //return RedirectToPage("Index");
        }

        public IActionResult OnPostLeaveTournament(Guid tournamentId)
        {
            if (User is not null && User.Identity.IsAuthenticated)
                AuthenticatedCustomer = _userManager.GetCustomerById(Guid.Parse(User.Identity.Name));
            try
            {
                _tournamentManager.TryUnRegisterPlayerToTournament(AuthenticatedCustomer.Id, tournamentId);
            }
            catch (InvalidOperationException e)
            {
                TempData["Error"] = e.Message;
            }
            catch (ArgumentException e)
            {
                TempData["Error"] = e.Message;
            }

            return Page();
        }
    }
}