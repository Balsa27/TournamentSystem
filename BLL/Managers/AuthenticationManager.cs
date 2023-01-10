using System.Security.Authentication;
using DAL;
using Modules.Entities;
using Modules.Interfaces.Manager;
using Modules.Tools;

namespace BLL.Managers
{
    public class AuthenticationManager 
    {
        private readonly UserManager _userManager; //change to interface and fix constructor to support tests

        public AuthenticationManager(UserManager userManager)
        {
            _userManager = userManager;
        }
       
        public bool AuthenticateCustomer(string username, string password)
        {
            foreach (Customer customer in _userManager.GetAllCustomers())
            {
                if (customer.Username == username && PasswordHasher.Validate(password, customer.Password))
                {
                    return true;
                }
            }
            throw new AuthenticationException("Invalid username or password");
        }

        public bool AuthenticateStaff(string username, string password)
        {
            foreach (Staff staff in _userManager.GetAllStaff())
            {
                if (staff.Username == username && staff.Password == password) //!!!!
                {
                    return true;
                }
            }
            throw new AuthenticationException("Invalid username or password");
        }
    }
}