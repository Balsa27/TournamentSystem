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
    public class MatchTests
    {
        Customer fisrtPlayer = new Customer("Balsa", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);
        Customer secondPlayer = new Customer("Balsa2", "Balsa", "Balsa", "Balsa", "balsa@gmail", Gender.MALE);

        [TestMethod]
        public void CreateMatchTest()
        {
            Match match = new Match(21, 14, fisrtPlayer, secondPlayer);
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void CreateMatchWithIdTest()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void MatchGettersTest()
        {
            Match match = new Match(Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"), 21, 14, fisrtPlayer, this.secondPlayer);
            Guid id = match.Id;
            int firstPlayerScore = 21;
            int secondPlayerScore = 14;
            Customer firstPlayer = match.FirstPlayer;
            Customer secondPlayer = match.SecondPlayer;
            Assert.AreEqual(id, Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"));
            Assert.AreEqual(firstPlayerScore, match.FirstPlayerScore);
            Assert.AreEqual(secondPlayerScore, match.SecondPlayerScore);
            Assert.AreEqual(firstPlayer, match.FirstPlayer);
            Assert.AreEqual(secondPlayer, match.SecondPlayer);
        }

        [TestMethod]
        public void CreateMatchWithNegativeScoreTest()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Match(Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"), -21, 14, fisrtPlayer, this.secondPlayer));
        }

        [TestMethod]
        public void CreateMatchWithNegativeScoreTest2()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Match(Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"), 21, -14, fisrtPlayer, this.secondPlayer));
        }

        [TestMethod]
        public void CreateMatchWithScoreOver30Test()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Match(Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"), 121, 14, fisrtPlayer, this.secondPlayer));
        }

        [TestMethod]
        public void CreateMatchWithScoreOver30Test2()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Match(Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"), 17,30, fisrtPlayer, this.secondPlayer));
        }

        [TestMethod]
        public void CreateMatchWithNullPlayerTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Match(Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"), 21, 14, null, this.secondPlayer));
        }

        [TestMethod]
        public void CreateMatchWithNullPlayerTest2()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Match(Guid.Parse("eafef2ac-9ddb-47f5-a3ac-7b474070e6f9"), 21, 14, this.fisrtPlayer, null));
        }

        [TestMethod]
        public void SetWinnerTest()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            Assert.IsNull(match.Winner);
            match.SetWinner(21, 3);
            Assert.IsNotNull(match.Winner);
        }

        [TestMethod]
        public void SetWinnerExceptionTest()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                match.SetWinner(22, 3);
            });
        }

        [TestMethod]
        public void SetWinnerExceptionTest2()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                match.SetWinner(21, 21);
            });
        }

        [TestMethod]
        public void SetWinnerExceptionTest3()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                match.SetWinner(3, 22);
            });
        }

        [TestMethod]
        public void SetWinnerExceptionTest4()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                match.SetWinner(32, 22);
            });
        }

        [TestMethod]
        public void SetWinnerWithGuidTest()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            match.SetWinner(match.FirstPlayer.Id);
            Assert.IsNotNull(match.Winner);
        }

        [TestMethod]
        public void GetWinnerId()
        {
            Match match = new Match(new Guid(), 21, 14, fisrtPlayer, secondPlayer);
            match.SetWinner(match.FirstPlayer.Id);
            Assert.IsNotNull(match.GetWinnerId());
        }
    }
}
