using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Tools;

namespace UnitTesting.Tests
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void GettersTest()
        {
            Customer customerTest = new Customer(Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989"), "Balsa3", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            Guid id = Guid.Parse("413e6467-ca0a-4952-b5dd-c043d1899989");
            string username = "Balsa3";
            string password = "Balsa";
            string firstName = "Balsa";
            string lastName = "Balsa";
            string email = "balsa@gmail";
            Gender gender = Gender.MALE;
            Assert.AreEqual(id, customerTest.Id);
            Assert.AreEqual(username, customerTest.Username);
            Assert.AreEqual(password, customerTest.Password);
            Assert.AreEqual(firstName, customerTest.FirstName);
            Assert.AreEqual(lastName, customerTest.LastName);
            Assert.AreEqual(email, customerTest.Email);
            Assert.AreEqual(gender, customerTest.Gender);
        }

        [TestMethod]
        public void CreateCustomerWithoutId()
        {
            Customer customerTest = new Customer("Balsa3", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            Assert.IsNotNull(customerTest);
        }

        [TestMethod]
        public void CreateCustomerWithId()
        {
            Customer customerTest = new Customer(new Guid(), "Balsa3", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            Assert.IsNotNull(customerTest);
        }

        [TestMethod]
        public void CreateCustomerWithoutUsername()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Customer(new Guid(), string.Empty, "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE));
        }

        [TestMethod]
        public void CreateCustomerWithoutPassword()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Customer(new Guid(), "string.Empty", string.Empty, "Balsa", "Balsa", "balsa@gmail", Gender.MALE));
        }

        [TestMethod]
        public void CreateCustomerWithoutFirstName()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Customer(new Guid(), "string.Empty", "string.Empty", string.Empty, "Balsa", "balsa@gmail", Gender.MALE));
        }

        [TestMethod]
        public void CreateCustomerWithoutLastName()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Customer(new Guid(), "string.Empty", "string.Empty", "Balsa", string.Empty, "balsa@gmail", Gender.MALE));
        }

        [TestMethod]
        public void CreateCustomerWithoutEmail()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Customer(new Guid(), "string.Empty", "string.Empty", "Balsa", "string.Empty", string.Empty, Gender.MALE));
        }
    }
}
