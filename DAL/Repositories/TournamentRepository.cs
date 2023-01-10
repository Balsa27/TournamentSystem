using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Interfaces.Repository;
using Modules.Tools;
using MySql.Data.MySqlClient;

namespace DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private string connString = "Server=studmysql01.fhict.local;Uid=dbi491971;Database=dbi491971;Pwd=12345678;";
        private readonly IUserRepository _userRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public TournamentRepository(IUserRepository userRepository, IScheduleRepository scheduleRepository)
        {
            _userRepository = userRepository;
            _scheduleRepository = scheduleRepository;
        }
        
        public List<Tournament> GetAllTournaments()
        {
            List<Tournament> allTournaments = new List<Tournament>();
            List<Customer> tournamentPlayers = new List<Customer>();
            List<Round> allTournamentRounds = new List<Round>();
            Gender gender;
            TournamentStatus status;
            using MySqlDataReader rdr = MySqlHelper.ExecuteReader(connString, 
                "SELECT id, tittle, maxPlayers, minPlayers, location, gender, status, startDate, endDate, TournamentSystem FROM tournament");
            while (rdr.Read())
            {
                if (Gender.TryParse(rdr.GetString("Gender"), out gender) &&
                    TournamentStatus.TryParse(rdr.GetString("status"), out status))
                {
                    Guid tournamentId = rdr.GetGuid("id");
                    
                    tournamentPlayers = _userRepository.GetAllTournamentPlayers(tournamentId); //information of the tournament will be repeated if it gets done with one query
                    allTournamentRounds = _scheduleRepository.GetAllTournamentRounds(tournamentId);
                    
                    Tournament tournament = new Tournament(
                        tournamentId,
                        rdr.GetString("tittle"),
                        rdr.GetString("location"),
                        rdr.GetInt32("minPlayers"),
                        rdr.GetInt32("maxPlayers"),
                        gender,
                        rdr.GetDateTime("startDate"),
                        rdr.GetDateTime("endDate"),
                        TournamentSystemFactory.GetTournamentSystem(rdr.GetString("TournamentSystem")),
                        status,
                        tournamentPlayers,
                        allTournamentRounds
                        );
                    allTournaments.Add(tournament);
                }
            }
            return allTournaments;
        }

        public void CreateTournament(Tournament tournament)
        {   
            MySqlHelper.ExecuteScalar(connString,
                "INSERT INTO tournament (id, tittle, maxPlayers, minPlayers, location, gender, status, startDate, endDate, TournamentSystem)" +
                "VALUES (@Id, @Tittle, @MaxPlayers, @MinPlayers, @Location, @Gender, @Status, @StartDate, @EndDate, @TournamentSystem)",
                new MySqlParameter("@Id", tournament.Id),
                new MySqlParameter("@Tittle", tournament.Tittle),
                new MySqlParameter("@MaxPlayers", tournament.MaxPlayers),
                new MySqlParameter("@MinPlayers", tournament.MinPlayers),
                new MySqlParameter("@Location", tournament.Location),
                new MySqlParameter("@Gender", tournament.Gender.ToString()),
                new MySqlParameter("@Status", tournament.Status ),
                new MySqlParameter("@StartDate", tournament.StartDate),
                new MySqlParameter("@EndDate", tournament.EndDate),
                new MySqlParameter("@TournamentSystem", tournament.TournamentSystem.Name)
                );
        }

        public void DeleteTournament(Tournament tournament)
        {
            MySqlHelper.ExecuteScalar(connString, "DELETE FROM tournament WHERE id = @Id",
                new MySqlParameter("@Id", tournament.Id));
        }

        public void UpdateTournament(Tournament tournament)
        {
            MySqlHelper.ExecuteScalar(connString, "UPDATE tournament SET tittle = @Tittle, " +
                                                  "maxPlayers = @MaxPlayers," +
                                                  "minPlayers = @MinPlayers," +
                                                  "location = @Location," +
                                                  "gender = @Gender," +
                                                  "status = @Status," +
                                                  "startDate = @StartDate," +
                                                  "endDate = @EndDate," +
                                                  "TournamentSystem = @TournamentSystem WHERE id = @Id",
                new MySqlParameter("@Id", tournament.Id),
                new MySqlParameter("@Tittle", tournament.Tittle),
                new MySqlParameter("@MaxPlayers", tournament.MaxPlayers),
                new MySqlParameter("@MinPlayers", tournament.MinPlayers),
                new MySqlParameter("@Location", tournament.Location),
                new MySqlParameter("@Gender", tournament.Gender.ToString()),
                new MySqlParameter("@Status", tournament.Status.ToString()),
                new MySqlParameter("@StartDate", tournament.StartDate),
                new MySqlParameter("@EndDate", tournament.EndDate),
                new MySqlParameter("@TournamentSystem", tournament.TournamentSystem.Name)
            );
        }

        public void RegisterPlayer(Customer customer, Tournament tournament)
        {
            MySqlHelper.ExecuteScalar(connString, "INSERT INTO tournament_players (tournamentId, customerId)" +
                                      "VALUES (@TournamentId, @CustomerId)",
                new MySqlParameter("@TournamentId", tournament.Id),
                new MySqlParameter("@CustomerId", customer.Id));
        }

        public void UnRegisterPlayer(Customer customer, Tournament tournament)
        {
            MySqlHelper.ExecuteScalar(connString, "DELETE FROM tournament_players WHERE tournamentId = @TournamentId AND customerId = @CustomerId",
                new MySqlParameter("@TournamentId", tournament.Id),
                new MySqlParameter("@CustomerId", customer.Id));
        }
                                                                    
     

        public void StartTournament(Tournament tournament)
        {
            MySqlHelper.ExecuteScalar(connString, "UPDATE tournament SET status = @Status WHERE id = @Id",
                new MySqlParameter("@Id", tournament.Id),
                new MySqlParameter("@Status", tournament.Status.ToString()));
        }
    }
}
