using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities.TournamentSystems;
using Modules.Tools;

namespace Modules.Entities
{
    public class Tournament
    {
        public Guid Id 
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string Tittle
        {
            get 
            {
                return title;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tittle cannot be empty");
                title = value;
            }
        }

        public int MaxPlayers
        {
            get
            {
                return maxPlayers;
            }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Maximum players cannot be 0");
                if (value <= minPlayers)
                    throw new ArgumentException("Maximum players cannot be less than minimum players");
                maxPlayers = value;
            }
        }

        public int MinPlayers
        {
            get
            {
                return minPlayers;
            }
            private set
            {
                if (value <= 1)
                    throw new ArgumentException("Minimum number of players should be at least 2");
                minPlayers = value;

            }
        }

        public string Location
        {
            get
            {
                return location;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Location cannot be empty");
                location = value;
            }
        }

        public Gender Gender
        {
            get
            {
                return gender;
            }
            private set
            {
                if (!(Enum.IsDefined(value))) 
                    throw new ArgumentException("Gender cannot be empty");
                gender = value;
            }
        }

        public TournamentSystem TournamentSystem
        {
            get
            {
                return tournamentSystem;
            }
            private set
            {
                tournamentSystem = value;
            }
        }

        public List<Customer> RegisteredPlayers
        {
            get
            {
                return registeredPlayers;
            }
            private set
            {
                registeredPlayers = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            private set
            {
                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            private set
            {
                if (value < startDate)
                    throw new ArgumentException("End date cannot be before start date");
                endDate = value;
            }
        }

        public TournamentStatus Status
        {
            get
            {
                return status;
            }
            private set
            {
                status = value;
            }
        }

        public List<Round> AllTournamentRounds
        {
            get
            {
                return allTournamentRounds;
            }
            private set
            {
                allTournamentRounds = value;
            }
        }


        public Tournament(Guid id, string title, string location, int minPlayers, 
            int maxPlayers, Gender gender, DateTime startDate, DateTime endDate, TournamentSystem tournamentSystem, TournamentStatus status, List<Customer> allPlayers)
        {
            Id = id;
            Tittle = title;
            Gender = gender;
            Location = location;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            StartDate = startDate;
            EndDate = endDate;
            TournamentSystem = tournamentSystem;
            Status = status;
            RegisteredPlayers = allPlayers;
        }

        public Tournament(Guid id, string title, string location, int minPlayers,
            int maxPlayers, Gender gender, DateTime startDate, DateTime endDate, TournamentSystem tournamentSystem, TournamentStatus status, List<Customer> allPlayers, List<Round> allTournamentRounds)
        {
            Id = id;
            Tittle = title;
            Gender = gender;
            Location = location;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            StartDate = startDate;
            EndDate = endDate;
            TournamentSystem = tournamentSystem;
            Status = status;
            RegisteredPlayers = allPlayers;
        }

        public Tournament(string title, string location, int minPlayers,
            int maxPlayers, Gender gender, DateTime startDate, DateTime endDate, TournamentSystem tournamentSystem)
        {
            Id = Guid.NewGuid();
            Tittle = title;
            Location = location;
            Gender = gender;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            StartDate = startDate;
            EndDate = endDate;
            TournamentSystem = tournamentSystem;
            Status = TournamentStatus.CREATED;
        }

        public void RegisterPlayer(Customer customer) 
        {
            registeredPlayers.Add(customer);
        }

        public void UnRegisterPlayer(Customer customer)
        {
            registeredPlayers.Remove(customer);
        }

        public void StartTournamnet() //push to db 
        {
            if(status != TournamentStatus.CREATED)
                throw new ArgumentException("Tournament already started");
            if (registeredPlayers.Count < minPlayers)
                throw new ArgumentException("Not enough players registered");
            status = TournamentStatus.STARTED;
            allTournamentRounds = tournamentSystem.ComputeAllAvailableRounds(registeredPlayers);
        }

        public List<Match> GetAllMatches()
        {
            return allTournamentRounds.SelectMany(r => r.Matches).ToList();
        }

        public override string ToString()
        {
            return $"Title: {Tittle}" +
                   $"\nMaxPlayers: {MaxPlayers}" +
                   $"\nMinPlayers: {MinPlayers}" +
                   $"\nLocation: {Location}" +
                   $"\nGender: {Gender}" +
                   $"\nStartDate: {startDate}" +
                   $"\nEndDate: {EndDate}" +
                   $"\nTournamentSystem: {TournamentSystem.Name}" +
                   $"\nStatus: {Status}";
        }

        private Guid id;
        private string title;
        private int maxPlayers;
        private int minPlayers;
        private string location;
        private Gender gender;
        private DateTime startDate;
        private DateTime endDate;
        private TournamentSystem tournamentSystem;
        private List<Customer> registeredPlayers = new List<Customer>();
        private List<Round> allTournamentRounds = new List<Round>();
        private TournamentStatus status;
    }
}
