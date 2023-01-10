using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Entities;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;
using Modules.Tools;
using Modules.Exceptions;

namespace BLL.Managers
{
    public class TournamentManager 
    {
        private readonly ITournamentRepository _tournamentRepository;
        private List<Tournament> _tournamentsCache;
        private readonly UserManager _userManager;
        private readonly ScheduleManager _scheduleManager;

        public TournamentManager(ITournamentRepository tournamentRepository, UserManager userManager, ScheduleManager scheduleManager)
        {
            _tournamentRepository = tournamentRepository;
            _userManager = userManager;  
            _tournamentsCache = _tournamentRepository.GetAllTournaments();
            _scheduleManager = scheduleManager;
        }

        public List<Tournament> GetAllTournaments()
        {
            if (_tournamentsCache is null || _tournamentsCache.Count == 0)
            {
                _tournamentsCache = _tournamentRepository.GetAllTournaments();
            }
            return _tournamentsCache;
        }

        public void CreateTournament(Tournament tournament)
        {
            if (GetAllTournaments().Any(t => t.Tittle == tournament.Tittle))
                throw new DuplicateEntryException("Tournament with this name already exists");
            _tournamentRepository.CreateTournament(tournament);
            _tournamentsCache.Add(tournament);
        }

        public void DeleteTournament(Tournament tournament) //on a tournament delete it should be good if the players are removed from db
        {
            _tournamentsCache.Remove(tournament);
            _tournamentRepository.DeleteTournament(tournament);
            //delete players as well 
        }

        public void UpdateTournament(Tournament tournament)
        {
            Tournament tournamentToUpdate = GetAllTournaments().FirstOrDefault(t => t.Id == tournament.Id);

            if (tournamentToUpdate is not null)
            {
                int counter = 0;
                foreach (Tournament t in GetAllTournaments())
                {
                    if (t.Tittle == tournament.Tittle)
                        counter++;
                    if(counter > 1)
                        throw new DuplicateEntryException("Tournament with this name already exists");
                }
                _tournamentsCache[_tournamentsCache.IndexOf(tournamentToUpdate)] = tournament;
                _tournamentRepository.UpdateTournament(tournament);
            }
        }

        public Tournament? GetTournamentById(Guid id)
        {
            return GetAllTournaments().Find(t => t.Id == id);
        }

        public List<Tournament>? GetAllCustomerTournaments(Guid id) //not passing an object becouse of asp
        {
            Customer customer = _userManager.GetCustomerById(id);

            if (customer is not null)
            {
                List<Tournament> allCustomerTournaments = new List<Tournament>();

                foreach (Tournament t in GetAllTournaments())
                {
                    if (t.RegisteredPlayers.Contains(customer) && t.Status == TournamentStatus.FINISHED)
                    {
                        allCustomerTournaments.Add(t);
                    }
                }
                return allCustomerTournaments;
            }

            return null; 
        }

        public bool TryRegisterPlayerToTournament(Guid userid, Guid tournamentId) //not passing an object becouse of asp
        {
            Customer? customer = _userManager.GetCustomerById(userid);
            Tournament? tournament = GetTournamentById(tournamentId);

            if (customer is not null)
            {
                if (tournament is not null)
                {
                    if (tournament.RegisteredPlayers.Find(c => c.Id == customer.Id) != null)
                        throw new InvalidOperationException("Player is already registered to this tournament");
                    if (tournament.StartDate - DateTime.Now.Date < TimeSpan.FromDays(7))
                        throw new InvalidOperationException("Cannot register 7 days before the tournament");
                    if (tournament.RegisteredPlayers.Count >= tournament.MaxPlayers)
                        throw new InvalidOperationException("Maximum number of players reached");
                    tournament.RegisterPlayer(customer);
                    _tournamentRepository.RegisterPlayer(customer, tournament);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool TryUnRegisterPlayerToTournament(Guid userId, Guid tournamentId)   
        {
            Customer customer = _userManager.GetCustomerById(userId);
            Tournament tournament = GetTournamentById(tournamentId);

            if (customer is not null)
            {
                if (tournament is not null)
                {
                    if (!tournament.RegisteredPlayers.Contains(customer))
                        throw new InvalidOperationException("You are not participating in this tournament anymore"); //test me
                    tournament.UnRegisterPlayer(customer);
                    _tournamentRepository.UnRegisterPlayer(customer, tournament);
                    return true;
                }

                return false;
            }
            return false;
        }

        public void StartTournament(Tournament tournament)
        {
            tournament.StartTournamnet();
            //_tournamentRepository.StartTournament(tournament);
            _scheduleManager.SaveSchedule(tournament);
        }
    }
}
