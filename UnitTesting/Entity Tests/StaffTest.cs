using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;

namespace UnitTesting.Entity_Tests
{
    [TestClass]
    public class StaffTest
    {
        [TestMethod]
        public void CreateStaffMemberTest()
        {
            Staff staff = new Staff(Guid.NewGuid(), "Balsa", "Balsa");
            Assert.IsNotNull(staff);
        }

        [TestMethod]
        public void CreateStaffMemberWitouthIdTest()
        {
            Staff staff = new Staff( "Balsa", "Balsa");
            Assert.IsNotNull(staff);
        }
    }
}
