using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Interfaces.Repository;
using MySql.Data.MySqlClient;
using Ubiety.Dns.Core;

namespace DAL.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private string connString = "Server=studmysql01.fhict.local;Uid=dbi491971;Database=dbi491971;Pwd=12345678;";
        IUserRepository _userRepository;

        public ScheduleRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<Round> GetAllTournamentRounds(Guid tournamentId)
        {
            List<Round> allTournamentRounds = new List<Round>();
            Dictionary<Guid, List<Match>> allMatches = new Dictionary<Guid, List<Match>>();
            string query =
                "SELECT r.roundId, m.id, m.firstPlayerId, m.secondPlayerId, m.firstPlayerScore, m.SecondPlayerScore, m.winnerId FROM round as r INNER JOIN matches as m ON r.roundId = m.roundId WHERE r.tournamentId = @TournamentId ORDER BY r.roundId";
            using MySqlDataReader rdr =
                MySqlHelper.ExecuteReader(connString, query, new MySqlParameter("@TournamentId", tournamentId));
            while (rdr.Read())
            {
                Guid roundId = rdr.GetGuid("roundId");
                if (!allMatches
                        .ContainsKey(
                            roundId)) //if dictionary contains the key, then we already have the matches for this round
                {
                    allMatches.Add(roundId, new List<Match>());

                }

                Customer firstPlayer = _userRepository.GetCustomerById(rdr.GetGuid("firstPlayerId"));
                Customer secondPlayer = _userRepository.GetCustomerById(rdr.GetGuid("secondPlayerId"));

                if (firstPlayer is not null && secondPlayer is not null)
                {
                    Match match = new Match(rdr.GetGuid("id"), rdr.GetInt32("firstPlayerScore"),
                        rdr.GetInt32("secondPlayerScore"), firstPlayer, secondPlayer);
                    if (!rdr.IsDBNull(rdr.GetOrdinal("winnerId")))
                    {
                        match.SetWinner(rdr.GetGuid("winnerId"));
                    }
                    allMatches[roundId].Add(match);
                }
            }

            foreach (List<Match> matches in allMatches.Values)
            {
                allTournamentRounds.Add(new Round(matches));
            }

            return allTournamentRounds;
        }

        public void SaveTournamentRounds(Tournament tournament)
        {
            List<Round> allTournamentRounds = tournament.AllTournamentRounds;
            
            StringBuilder sCommand = new StringBuilder("INSERT INTO round (tournamentId, roundId) VALUES ");
            using (MySqlConnection mConnection = new MySqlConnection(connString))
            {
                List<string> Rows = new List<string>();
                foreach (Round round in allTournamentRounds)
                {
                    Rows.Add(string.Format("('{0}','{1}')", MySqlHelper.EscapeString(tournament.Id.ToString()),
                        MySqlHelper.EscapeString(round.Id.ToString())));
                }

                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }

                StringBuilder stCommand =
                    new StringBuilder(
                        "INSERT INTO matches (id, firstPlayerScore, secondPlayerScore, firstPlayerId, secondPlayerId, roundId) VALUES ");
                Rows.Clear();
                foreach (Round round in allTournamentRounds)
                {
                    foreach (Match match in round.Matches)
                    {
                        Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}')",
                            MySqlHelper.EscapeString(match.Id.ToString()),
                            MySqlHelper.EscapeString(match.FirstPlayerScore.ToString()),
                            MySqlHelper.EscapeString(match.SecondPlayerScore.ToString()),
                            MySqlHelper.EscapeString(match.FirstPlayer.Id.ToString()),
                            MySqlHelper.EscapeString(match.SecondPlayer.Id.ToString()),
                            MySqlHelper.EscapeString(round.Id.ToString())));
                    }
                }
                
                stCommand.Append(string.Join(",", Rows));
                stCommand.Append(";");
                using (MySqlCommand myCmd = new MySqlCommand(stCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveMatchResults(Match currentMatch, int firstPlayerScore, int secondPlayerScore)
        {
            MySqlHelper.ExecuteScalar(connString,
                "UPDATE matches SET firstPlayerScore = @FirstPlayerScore, secondPlayerScore = @SecondPlayerScore, winnerId = @WinnerId WHERE id = @Id",
                new MySqlParameter("@FirstPlayerScore", firstPlayerScore),
                new MySqlParameter("@SecondPlayerScore", secondPlayerScore),
                new MySqlParameter("@Id", currentMatch.Id),
                new MySqlParameter("@WinnerId", currentMatch.GetWinnerId()));
        }
    }
}