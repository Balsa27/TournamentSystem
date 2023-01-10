using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Exceptions;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;

namespace BLL.Managers
{
    public class UserManager 
    {
        private readonly IUserRepository _userRepository;
        private List<Customer> _customersCache;
        private List<Staff> _staffCache;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _customersCache = _userRepository.GetAllCustomers();
            _staffCache = _userRepository.GetAllStaff();
        }

        public List<Customer> GetAllCustomers()
        {
            if (_customersCache is null || _customersCache.Count == 0)
            {
                _customersCache = _userRepository.GetAllCustomers();
            }
            return _customersCache;
        }

        public List<Staff> GetAllStaff()  
        {
            if (_staffCache is null || _staffCache.Count == 0)
            {
                _staffCache = _userRepository.GetAllStaff();
            }
            return _staffCache;
        }

        public void CreateCustomer(Customer customer)
        {
            if (GetAllCustomers().Any(c => c.Email == customer.Email))
            {
                throw new ArgumentException("Email is already in use");
            } 
            if (GetAllCustomers().Any(c => c.Username == customer.Username))
            {
                throw new ArgumentException("Username is already in use");
            }
            else
            {
                _userRepository.CreateCustomer(customer);
                _customersCache.Add(customer);
            }
        }

        public void DeleteCustomer(Customer customer)
        {
            _customersCache.Remove(customer);
            _userRepository.DeleteCustomer(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            Customer cust = GetAllCustomers().FirstOrDefault(c => c.Id == customer.Id); //finds a customer with the condition

            if (cust is not null)
            { 
                _customersCache.Remove(cust);
                
                if (GetAllCustomers().Any(c => c.Email == customer.Email))
                {
                    throw new DuplicateEntryException("Email is already in use");
                }
                if (GetAllCustomers().Any(c => c.Username == customer.Username))
                {
                    throw new DuplicateEntryException("Username is already in use");
                }
                else
                {
                    _customersCache.Add(customer);
                    _userRepository.UpdateCustomer(customer);
                }
            }
            else
            {
                 throw new ArgumentException("Customer not found");
            }
        }

        public Customer? GetCustomerByUsername(string username)
        {
            return GetAllCustomers().Find(c => c.Username == username);
        }

        public Customer? GetCustomerById(Guid id)
        {
            return GetAllCustomers().Find(c => c.Id == id);
        }
    }
}
