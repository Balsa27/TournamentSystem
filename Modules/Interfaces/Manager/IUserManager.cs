using Modules.Entities;

namespace Modules.Interfaces.Manager;

public interface IUserManager
{
    public List<Customer> GetAllCustomers();
    public List<Staff> GetAllStaff();
    public void CreateCustomer(Customer customer);
    public void DeleteCustomer(Customer customer);
    public void UpdateCustomer(Customer customer);
    public Customer GetCustomerByUsername(string username);
    public Customer GetCustomerById(Guid id);

}