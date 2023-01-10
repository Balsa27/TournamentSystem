using Modules.Entities;

namespace DAL.Repositories;

public interface IScheduleRepository
{
    public List<Round> GetAllTournamentRounds(Guid tournamentId);
    public void SaveTournamentRounds(Tournament tournament);
    public void SaveMatchResults(Match match, int firstPlayerScore, int secondPlayerScore);

}