using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using BLL.Managers;
using DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Tools;
using UnitTesting.Manager_test.MockDatabaseClasses;

namespace UnitTesting.Manager_test.Tests
{
    [TestClass]
    public class AuthenticationManagerTest
    {
        private readonly UserManager _userManager;
        private readonly AuthenticationManager _authenticationManager;
        private Customer fisrtPlayer = new Customer("Balsa", PasswordHasher.HashPassword("Balsa"), "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private Staff staffMember = new Staff(new Guid(), "Mark", "Joe");
        private Staff staffMember2 = new Staff(new Guid(), "Marko", "Joeo");
        private readonly List<Staff> allStaff;
        private readonly List<Customer> allCustomers;


        public AuthenticationManagerTest()
        {
            _userManager = new UserManager(new UserRepositoryMock());
            _authenticationManager = new AuthenticationManager(_userManager);
            allStaff = _userManager.GetAllStaff();
            allCustomers = _userManager.GetAllCustomers();
        }

        [TestMethod]
        public void AuthenticateStaffTest()
        {
            allStaff.Add(staffMember);
            Assert.IsTrue(_authenticationManager.AuthenticateStaff(staffMember.Username, staffMember.Password));
        }

        [TestMethod]
        public void AuthenticateStaffFailTest()
        {
            allStaff.Add(staffMember);
        }

        [TestMethod]
        public void AuthenticateCustomerTest()
        {
            allCustomers.Add(fisrtPlayer);
            string password = "Balsa";
            bool passwordToBeValidated = PasswordHasher.Validate(password, fisrtPlayer.Password);
            Assert.IsTrue(passwordToBeValidated);
        }

        [TestMethod]
        public void AuthenticateCustomerFailTest()
        {
            List<Customer> allCustomers = _userManager.GetAllCustomers();
            allCustomers.Add(fisrtPlayer);
            Assert.ThrowsException<AuthenticationException>(() => _authenticationManager.AuthenticateCustomer(secondPlayer.Username, secondPlayer.Password));
        }
    }
}
