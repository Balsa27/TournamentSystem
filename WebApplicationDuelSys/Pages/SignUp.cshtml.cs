using System.Security.Claims;
using BLL.Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modules.Entities;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;
using Modules.Tools;
using WebApplicationDuelSys.DTOs;

namespace WebApplicationDuelSys.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public SignUpDTO singUpDTO { get; set; }
        private readonly UserManager _userManager;

        public SignUpModel(UserManager userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                return SignUpCustomer();
            }
            else
            {
                return Page();
            }
        }
        
        private IActionResult SignUpCustomer()
        {
            singUpDTO.Id = Guid.NewGuid();
            string hashedPassword = PasswordHasher.HashPassword(singUpDTO.Password);
            Customer customer = new Customer(singUpDTO.Id, singUpDTO.Username, hashedPassword, singUpDTO.FirstName,
                singUpDTO.LastName, singUpDTO.Email, singUpDTO.Gender);
            _userManager.CreateCustomer(customer);
            SetupSessionCookie();
            return Redirect("/index");
        }

        private void SetupSessionCookie()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, singUpDTO.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, "User"));

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(new ClaimsPrincipal(claimIdentity));
        }
    }
}
