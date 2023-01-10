namespace Modules.Entities
{
    public class User
    {
        public Guid Id
        {
            get
            {
                return id;
            }
            private set
            {
                id = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty");
                username = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password cannot be empty");
                password = value;
            }
        }

        protected User()
        {

        }

        protected User(Guid id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        protected User(string username, string password)
        {
            Username = username;
            Password = password;
            Id = Guid.NewGuid(); //creates an id automatically
        }

        private Guid id;
        private string username;
        private string password;
    }
}