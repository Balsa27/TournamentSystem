using System.Globalization;
using Modules.Entities;
using Modules.Interfaces.Repository;
using Modules.Tools;
using MySql.Data.MySqlClient;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        string connString = "Server=studmysql01.fhict.local;Uid=dbi491971;Database=dbi491971;Pwd=12345678;";

        public List<Customer> GetAllCustomers()
        {
            List<Customer> allCustomers = new List<Customer>();
            Gender gender;
            using MySqlDataReader rdr = MySqlHelper.ExecuteReader(connString, "SELECT id, username, password, firstName, lastName, email, gender FROM customer");
            while (rdr.Read())
            {
                if (Enum.TryParse(rdr.GetString("gender"), out gender))
                {
                    Customer customer = new Customer(
                        rdr.GetGuid("id"),
                        rdr.GetString("username"),
                        rdr.GetString("password"),
                        rdr.GetString("firstname"),
                        rdr.GetString("lastname"),
                        rdr.GetString("email"),
                        gender
                    );
                    allCustomers.Add(customer);
                }
            }
            return allCustomers;
        }
        
        public List<Staff> GetAllStaff()
        {
            List<Staff> allStaff = new List<Staff>();
            using MySqlDataReader rdr = MySqlHelper.ExecuteReader(connString, "SELECT id, username, password FROM stuff");
            while (rdr.Read())
            {
                Staff staff = new Staff(
                    rdr.GetGuid("id"),
                    rdr.GetString("username"),
                    rdr.GetString("password")
                );
                allStaff.Add(staff);
            }
            return allStaff;
        }

        public void CreateCustomer(Customer customer)
        {
            
            MySqlHelper.ExecuteScalar(connString,
                "INSERT INTO customer (id, username, password, firstName, lastName, email, gender) " +
                "VALUES (@Id, @Username, @Password, @FirstName, @LastName, @Email, @Gender)",
                new MySqlParameter("@Username", customer.Username),
                new MySqlParameter("@Password", customer.Password),
                new MySqlParameter("@FirstName", customer.FirstName),
                new MySqlParameter("@LastName", customer.LastName),
                new MySqlParameter("@Email", customer.Email),
                new MySqlParameter("@Gender", Convert.ToString(customer.Gender)),
                    new MySqlParameter("@Id", customer.Id));
        }

        public void DeleteCustomer(Customer customer)
        {
            MySqlHelper.ExecuteScalar(connString,
                "DELETE FROM customer WHERE @id = id",
                new MySqlParameter("@id", customer.Id)
            );
        }

        public void UpdateCustomer(Customer customer)
        {
            MySqlHelper.ExecuteScalar(connString,
                "UPDATE customer SET " +
                "username = @Username, " +
                "password = @Password, " +
                "firstName = @FirstName, " +
                "lastName = @LastName, " +
                "email = @Email," +
                "gender = @Gender WHERE id = @id",
                new MySqlParameter("@id", customer.Id),
                new MySqlParameter("@Password", customer.Password),
                new MySqlParameter("@FirstName", customer.FirstName),
                new MySqlParameter("@Username", customer.Username),
                new MySqlParameter("@Email", customer.Email),
                new MySqlParameter("@LastName", customer.LastName),
                new MySqlParameter("@Gender", Convert.ToString(customer.Gender)));
        }

        public Customer GetCustomerById(Guid customerId)
        {
            Customer customer = null;
            using MySqlDataReader rdr = MySqlHelper.ExecuteReader(connString,
                "SELECT id, username, password, firstName, lastName, email, gender FROM customer WHERE id = @Id",
                new MySqlParameter("@Id", customerId));
            Gender gender;
            while (rdr.Read())
            {
                if (Enum.TryParse(rdr.GetString("gender"), out gender))
                {
                    customer = new Customer(
                        rdr.GetGuid("id"),
                        rdr.GetString("username"),
                        rdr.GetString("password"),
                        rdr.GetString("firstname"),
                        rdr.GetString("lastname"),
                        rdr.GetString("email"),
                        gender
                    );
                }
            }

            return customer;
        }
        public List<Customer> GetAllTournamentPlayers(Guid id)
        {
            List<Customer> allTournamentPlayers = new List<Customer>();
            using MySqlDataReader rdr = MySqlHelper.ExecuteReader(connString,
                "SELECT c.id, c.username, c.password, c.firstName, c.lastName, c.email, c.gender FROM customer as c INNER JOIN tournament_players as tp ON c.id = tp.customerId WHERE tp.tournamentId = @TournamentId",
                new MySqlParameter("@TournamentId", id));
            Gender gender;
            while (rdr.Read())
            {
                if (Enum.TryParse(rdr.GetString("gender"), out gender))
                {
                    Customer customer = new Customer(
                        rdr.GetGuid("id"),
                        rdr.GetString("username"),
                        rdr.GetString("password"),
                        rdr.GetString("firstname"),
                        rdr.GetString("lastname"),
                        rdr.GetString("email"),
                        gender
                    );
                    allTournamentPlayers.Add(customer);
                }
            }
            return allTournamentPlayers;
        }
    }
}