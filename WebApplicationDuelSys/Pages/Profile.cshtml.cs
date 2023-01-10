using BLL.Managers;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modules.Entities;
using Modules.Interfaces.Manager;
using Microsoft.AspNetCore.Authentication;
using Modules.Exceptions;
using Modules.Interfaces.Repository;
using Modules.Tools;
using WebApplicationDuelSys.DTOs;

namespace WebApplicationDuelSys.Pages
{
    [Authorize(Roles = "User")] 
    public class ProfileModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly TournamentManager _tournamentManager;
        private readonly ScheduleManager _scheduleManager;

        [BindProperty] public Customer? Customer { get; set; } = new Customer();

        public ProfileModel(UserManager userManager, TournamentManager tournamentManager, ScheduleManager scheduleManager)
        {
            _userManager = userManager;
            _tournamentManager = tournamentManager;
            _scheduleManager = scheduleManager;
        }

        public void OnGet()
        {
            var id = HttpContext.User.Identity.Name;
            Customer = _userManager.GetCustomerById(Guid.Parse(id));
        }

        public IActionResult OnPostSignOut()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return new RedirectToPageResult("/Index");
        }

        public IActionResult OnPostUpdateCustomerInfo()
        {
            var id = HttpContext.User.Identity.Name;
            Customer = _userManager.GetCustomerById(Guid.Parse(id));
            if (Customer is null)
                return Page();
            //Gender gender2;
            //Gender.TryParse(Request.Form["gender"], out gender2);
            Gender gender = (Gender)Enum.Parse(typeof(Gender), Request.Form["gender"]); //very quality! 
            Customer = new Customer(Customer.Id, Request.Form["Customer.Username"], Request.Form["Customer.Password"],
                Request.Form["Customer.FirstName"], Request.Form["Customer.LastName"], Request.Form["Customer.Email"], gender);
            try
            {
                _userManager.UpdateCustomer(Customer);
            }
            catch (DuplicateEntryException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return Page();
        }

        public List<Tournament> GetAllCustomerTournaments()
        {
            return _tournamentManager.GetAllCustomerTournaments(Guid.Parse(User.Identity.Name)); //very confused why this does not work
        }
    }
}
