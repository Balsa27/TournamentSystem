using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Entities
{
    public class Match
    {
        public int FirstPlayerScore
        {
            get
            {
                return this.firstPlayerScore;
            }
            private set
            {
                if(value < 0)
                    throw new ArgumentException("Score cannot be negative");
                if (value >= 30)
                    throw new ArgumentException("Score can not be over 30");
                this.firstPlayerScore = value;
            }
        }

        public int SecondPlayerScore
        {
            get
            {
                return this.secondPlayerScore;
            }
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Score cannot be negative");
                if (value >= 30)
                    throw new ArgumentException("Score can not be over 30");
                this.secondPlayerScore = value;
            }
        }

        public Customer FirstPlayer
        {
            get
            {
                return this.firstPlayer;
            }
            private set
            {
                if (value is null)
                    throw new ArgumentNullException("Player cannot be null");
                this.firstPlayer = value;
            }
        }

        public Customer SecondPlayer
        {
            get
            {
                return this.secondPlayer;
            }
            private set
            {
                if (value is null)
                    throw new ArgumentNullException("Player cannot be null");
                this.secondPlayer = value;
            }
        }

        public Customer Winner
        {
            get
            {
                return winner;
            }
            private set
            {
                winner = value;
            }
        }

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

        public Match(int firstPlayerScore, int secondPlayerScore, Customer firstPlayer, Customer secondPlayer)
        {
            this.FirstPlayerScore = firstPlayerScore;
            this.SecondPlayerScore = secondPlayerScore;
            this.FirstPlayer = firstPlayer;
            this.SecondPlayer = secondPlayer;
            this.id = Guid.NewGuid();
            winner = null; // no winner yet
        }

        public Match(Guid id, int firstPlayerScore, int secondPlayerScore, Customer firstPlayer, Customer secondPlayer)
        {
            this.FirstPlayerScore = firstPlayerScore;
            this.SecondPlayerScore = secondPlayerScore;
            this.FirstPlayer = firstPlayer;
            this.SecondPlayer = secondPlayer;
            this.Id = id;

            winner = null; // no winner yet
        }

        public override string ToString() 
        {
            return $"{firstPlayer.Username} vs {secondPlayer.Username}";
        }

        public void SetWinner(int firstPlayerScore, int secondPlayerScore) 
        {
            if (firstPlayerScore < 21 && secondPlayerScore < 21)
                throw new ArgumentException("Score cannot be less than 21");
            if (firstPlayerScore > 31 || secondPlayerScore > 31)
                throw new ArgumentException("Score cannot be over 30");
            if (firstPlayerScore == 30 && secondPlayerScore == 29)
            {
                winner = this.FirstPlayer;
                this.firstPlayerScore = firstPlayerScore;
                this.secondPlayerScore = secondPlayerScore;
                return;
            }
            if (firstPlayerScore == 29 && secondPlayerScore == 30)
            {
                winner = this.SecondPlayer;
                this.firstPlayerScore = firstPlayerScore;
                this.secondPlayerScore = secondPlayerScore;
                return;
            }
            if (firstPlayerScore == 21 && secondPlayerScore < 20)
            {
                winner = this.FirstPlayer;
                this.firstPlayerScore = firstPlayerScore;
                this.secondPlayerScore = secondPlayerScore;
                return;
            }
            if (secondPlayerScore == 21 && firstPlayerScore < 20)
            {
                winner = this.SecondPlayer;
                this.firstPlayerScore = firstPlayerScore;
                this.secondPlayerScore = secondPlayerScore;
                return;
            }
            if (Math.Abs(firstPlayerScore - secondPlayerScore) != 2)
                throw new ArgumentException("Score difference has to be at exactly 2");
            if (firstPlayerScore > secondPlayerScore)
            {
                winner = this.FirstPlayer;
                this.firstPlayerScore = firstPlayerScore;
                this.secondPlayerScore = secondPlayerScore;
            }
            else
            {
                winner = this.secondPlayer;
                this.firstPlayerScore = firstPlayerScore;
                this.secondPlayerScore = secondPlayerScore;
            }
        }
        
        public void SetWinner(Guid? winnerId)
        {
            if (winnerId == this.FirstPlayer.Id)
                winner = this.FirstPlayer;
            else if (winnerId == this.SecondPlayer.Id)
                winner = this.SecondPlayer;
        }

        public Guid? GetWinnerId()
        {
            if (winner is null)
                return null;
            return winner.Id;
        }

        public override bool Equals(object? obj)
        {
            Match match = obj as Match;
            if (match is null)
                return false;
            if (this.FirstPlayer.Id == match.FirstPlayer.Id && this.SecondPlayer.Id == match.SecondPlayer.Id)
                return true;
            return false;
        }
 
        private Customer firstPlayer;
        private Customer secondPlayer;
        private Customer winner;
        private int firstPlayerScore;
        private int secondPlayerScore;
        private Guid id;
    }
}
