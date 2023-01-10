using Modules.Entities;

namespace Modules.Interfaces.Repository;

public interface IUserRepository
{
    public List<Customer> GetAllCustomers();
    public List<Staff> GetAllStaff();
    public void CreateCustomer(Customer customer);
    public void DeleteCustomer(Customer customer);
    public void UpdateCustomer(Customer customer);
    public Customer GetCustomerById(Guid id);
    public List<Customer> GetAllTournamentPlayers(Guid id);
}