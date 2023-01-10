using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modules.Entities;
using Modules.Tools;

namespace UnitTesting.Entity_Tests
{
    [TestClass]
    public class RoundTests
    {
        [TestMethod]
        public void RoundConstructorTest()
        {
            List<Customer> players = new List<Customer>();
            Customer fisrtPlayer = new Customer("Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            Customer thirdPlayer = new Customer("Balsa3", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
            Customer forthPlayer = new Customer("Bals2a3", "B24lsa", "Ba42lsa", "Ba42lsa", "bal42sa@gmail", Gender.MALE);
            Match match = new Match(new Guid(), 21, 10, fisrtPlayer, secondPlayer);
            Match match2 = new Match(new Guid(), 21, 10, thirdPlayer, forthPlayer);
            List<Match> allMatches = new List<Match>();
            allMatches.Add(match);
            allMatches.Add(match2);
            Round round = new Round(allMatches);
            Assert.IsNotNull(round);
        }
    }
}
