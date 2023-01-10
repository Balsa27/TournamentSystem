using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using Modules.Entities;
using Org.BouncyCastle.X509;

namespace BLL.Managers
{
    public class ScheduleManager //schedule (round, match database connction)
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleManager(IScheduleRepository repository)
        {
            _scheduleRepository = repository;
        }

        public void SaveSchedule(Tournament tournament) //gonna push to the database all the rounds and matches of the schedule
        {
            _scheduleRepository.SaveTournamentRounds(tournament);
        }

        public void SetPlayerScores(Tournament tournament, Match currentMatch, int firstPlayerScore, int secondPlayerScore)
        {
            List<Match> matches = tournament.GetAllMatches();
            Match match = matches.Find(x => x.Id == currentMatch.Id);
            if (match is not null)
            {
                match.SetWinner(firstPlayerScore, secondPlayerScore);
                _scheduleRepository.SaveMatchResults(match, firstPlayerScore, secondPlayerScore);
            }
            else
            {
                throw new InvalidOperationException("Match not found");
            }
        }

        public List<Round> GetAllAvailableRounds(Tournament tournament) //used in asp once, should be done with tournament.AllRounds
        {
            return _scheduleRepository.GetAllTournamentRounds(tournament.Id); 
        }
    }
}
