using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Interfaces.Repository;

namespace UnitTesting.Manager_test.MockDatabaseClasses
{
    public class UserRepositoryMock : IUserRepository
    {
        public List<Customer> GetAllCustomers()
        {
            return new List<Customer>();
        }

        public List<Staff> GetAllStaff()
        {
            return new List<Staff>();
        }

        public void CreateCustomer(Customer customer)
        {
        }

        public void DeleteCustomer(Customer customer)
        {
        }

        public void UpdateCustomer(Customer customer)
        {
        }

        public Customer GetCustomerById(Guid id)
        {
            return new Customer();
        }

        public List<Customer> GetAllTournamentPlayers(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
