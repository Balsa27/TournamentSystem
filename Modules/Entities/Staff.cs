using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Entities
{
    public class Staff : User
    {
        public Staff(Guid id, string username, string password) : base(id, username, password)
        {
                
        }

        public Staff(string username, string password) : base(username, password)
        {

        }
    }
}
