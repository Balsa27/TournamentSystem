using System.ComponentModel;
using System.Security.Authentication;
using System.Security.Claims;
using BLL.Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;
using WebApplicationDuelSys.DTOs;

namespace WebApplicationDuelSys.Pages
{
    public class LogIn : PageModel
    {
        [BindProperty]
        public LogInDTO logInDTO { get; set; }

        private readonly AuthenticationManager _authenticationManager;
        private readonly UserManager _userManager;

        public LogIn(IUserRepository _userRepository)
        {
            _userManager = new UserManager(_userRepository);
            _authenticationManager = new AuthenticationManager(_userManager);
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                return LogInCustomer();
            }
            else
            {
                return Page();
            }
        }

        private ActionResult LogInCustomer()
        {
            try
            {
                _authenticationManager.AuthenticateCustomer(logInDTO.Username, logInDTO.Password);
                var id = _userManager.GetCustomerByUsername(logInDTO.Username).Id;
                Auth(id);
                return RedirectToPage("/Index");
            }
            catch (AuthenticationException ex)
            {
                TempData["Error"] = ex.Message;
                return Page();
            }
        }

        public async void Auth(Guid id)
        {
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(28)
            };

            var claimsss = new List<Claim>();
            claimsss.Add(new Claim(ClaimTypes.Name, id.ToString()));
            claimsss.Add(new Claim(ClaimTypes.Role, "User"));

            var claimsIdentity = new ClaimsIdentity(claimsss, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}
