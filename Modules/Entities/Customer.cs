using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Tools;

namespace Modules.Entities
{
    public class Customer : User
    {
        public string FirstName
        {
            get
            {
                return firstName;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be empty");
                this.firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be empty");
                this.lastName = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty");
                this.email = value;
            }
        }

        public Gender Gender
        {
            get
            {
                return this.gender;
            }
            private set
            {
                if (!Enum.IsDefined(value))
                    throw new ArgumentException("Gender is not valid");
                else
                    this.gender = value;
            }
        }

        public Customer()
        {

        }

        public Customer(Guid id, string username, string password, string firstname, string lastName, string email, Gender gender)
            : base(id, username, password)
        {
            FirstName = firstname;
            LastName = lastName;
            Email = email;
            Gender = gender;
        }

        public Customer(string username, string password, string firstname, string lastName, string email, Gender gender)
            : base(username, password)
        {
            FirstName = firstname;
            LastName = lastName;
            Email = email;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"Username: {Username}";
        }

        public override bool Equals(object? obj) 
        {
            Customer customer = obj as Customer;
            if (customer.Id.Equals(Id))
            {
                return true;
            }
            return false;
        }

        private string firstName;
        private string lastName;
        private string email;
        private Gender gender;
    }
}
