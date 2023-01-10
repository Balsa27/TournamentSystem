using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Interfaces.Repository;
using Modules.Tools;
using UnitTesting.Manager_test.MockDatabaseClasses;

namespace UnitTesting.Manager_test
{
    [TestClass]
    public class UserManagerTest
    {
        private readonly UserManager _userManager;
        private Customer fisrtPlayer = new Customer("Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        private Staff staffMember = new Staff(new Guid(), "Mark", "Joe");
        private Staff staffMember2 = new Staff(new Guid(), "Marko", "Joeo");
        private List<Customer> customers;
        private List<Staff> staff;

        public UserManagerTest()
        {
            _userManager = new UserManager(new UserRepositoryMock());
            customers = _userManager.GetAllCustomers();
            staff = _userManager.GetAllStaff();
        }

        private void AddCustomersToAList()
        {
            customers.Add(fisrtPlayer);
            customers.Add(secondPlayer);
        }

        private void AddStaffToAList()
        {
            staff.Add(staffMember);
            staff.Add(staffMember2);
        }

        [TestMethod]
        public void GetAllCustomersCountTest()
        {
            AddCustomersToAList();
            Assert.IsTrue(customers.Count == 2);
        }

        [TestMethod]
        public void GetAllCustomersCountTestTwo() //same as first pretty much
        {
            AddCustomersToAList();
            Assert.IsTrue(customers.Contains(fisrtPlayer));
            Assert.IsTrue(customers.Contains(secondPlayer));
        }

        [TestMethod]
        public void CheckIfGetAllUsersIsNullTest()
        {
            Assert.IsTrue(customers.Count == 0);
        }

        [TestMethod]
        public void CheckIfGetAllUsersIsNotNullTest()
        {
            AddCustomersToAList();
            Assert.IsNotNull(customers);
        }

        [TestMethod]
        public void GetAllStaffCountTest()
        {
            AddStaffToAList();
            Assert.IsTrue(staff.Count == 2);
        }

        [TestMethod]
        public void CheckIfGetAllStaffIsNullTest()
        {
            Assert.IsTrue(staff.Count == 0);
        }

        [TestMethod]
        public void CheckIfGetAllStaffIsNotNullTest()
        {
            AddStaffToAList();
            Assert.IsNotNull(staff);
        }

        [TestMethod]
        public void CreateCustomerTest()
        {
            Customer newCustomer =
                new Customer(new Guid(), "Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            _userManager.CreateCustomer(newCustomer);
            Assert.IsTrue(_userManager.GetAllCustomers().Contains(newCustomer));
        }

        [TestMethod]
        public void CreateCustomerWithAnUsernameThatAlreadyExists()
        {
            Customer newCustomer =
                new Customer(new Guid(), "Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            _userManager.CreateCustomer(newCustomer);
            Customer falseCustomer = new Customer(new Guid(), "Balsa", "Balsa", "Balsa", "Balsa", "balsa123@gmail",
                Gender.MALE);
            Assert.ThrowsException<ArgumentException>(() => { _userManager.CreateCustomer(falseCustomer); });
        }

        [TestMethod]
        public void CreateCustomerWithAnEmailThatAlreadyExists()
        {
            Customer newCustomer =
                new Customer(new Guid(), "Balsa123", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            _userManager.CreateCustomer(newCustomer);
            Customer falseCustomer =
                new Customer(new Guid(), "Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            Assert.ThrowsException<ArgumentException>(() => { _userManager.CreateCustomer(falseCustomer); });
        }

        [TestMethod]
        public void UpdateCustomerTest()
        {
            Customer newCustomer =
                new Customer(new Guid(), "Balsa123", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            _userManager.CreateCustomer(newCustomer);
            _userManager.UpdateCustomer(newCustomer);
            Assert.AreEqual(newCustomer, _userManager.GetAllCustomers()[0]);
        }

        [TestMethod]
        public void DeleteCustomer()
        {
            customers.Add(fisrtPlayer);
            _userManager.DeleteCustomer(fisrtPlayer);
            CollectionAssert.DoesNotContain(customers, fisrtPlayer);
            Assert.IsTrue(customers.Count == 0);
        }

        [TestMethod]
        public void GetCustomerByIdTest()
        {
            customers.Add(fisrtPlayer);
            Customer customer = _userManager.GetCustomerById(fisrtPlayer.Id);
            Assert.AreEqual(fisrtPlayer, customer);
            Assert.AreEqual(customer, customers[0]);
            
        }

        [TestMethod]
        public void GetCustomerByUsernameTest()
        {
            customers.Add(fisrtPlayer);
            Customer customer = _userManager.GetCustomerByUsername(fisrtPlayer.Username);
            Assert.AreEqual(customer, fisrtPlayer);
            Assert.AreEqual(customer, customers[0]);
        }
    }
}
