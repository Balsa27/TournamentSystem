using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL.Managers;
using DAL.Repositories;
using Modules.Entities;
using Modules.Entities.TournamentSystems;
using Modules.Exceptions;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;
using Modules.Tools;

namespace SynthesissAssignment
{
    public partial class StaffForm : Form
    {
        private readonly TournamentManager _tournamentManager;
        private readonly UserManager _userManager;
        private readonly ScheduleManager _scheduleManager;
        private readonly BindingList<Tournament> _allTournaments;
        private readonly List<Type> _tournamentTypes;
        
        public StaffForm(ITournamentRepository _tournamentRepository, IUserRepository _userRepository, IScheduleRepository _scheduleRepository)
        {
            InitializeComponent();
            _userManager = new UserManager(_userRepository);
            _scheduleManager = new ScheduleManager(_scheduleRepository);
            _tournamentManager = new TournamentManager(_tournamentRepository, _userManager, _scheduleManager);
            _allTournaments = new BindingList<Tournament>(_tournamentManager.GetAllTournaments());
            _scheduleManager = new ScheduleManager(_scheduleRepository);
            lbTournaments.DisplayMember = "Tittle"; 
            lbTournaments.DataSource = _allTournaments;
            tbId.ReadOnly = true;
            cbGender.DataSource = Enum.GetValues(typeof(Gender));
            _tournamentTypes = Assembly.GetAssembly(typeof(TournamentSystem))
                .GetTypes()
                .Where(t => t.BaseType == typeof(TournamentSystem))
                .ToList();
            cbTorunamentType.DataSource = _tournamentTypes;
            cbTorunamentType.DisplayMember = "Name";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Tournament tournament = (Tournament)lbTournaments.SelectedItem;
            if (tournament is not null)
            {
                MessageBox.Show(tournament.ToString());
            }
            else
            {
                MessageBox.Show("No tournament selected");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Tournament tournamentToBeCreated = new Tournament(
                    tbTitle.Text, tbLocation.Text,
                    Convert.ToInt32(nudMinPlayers.Value),
                    Convert.ToInt32(nupMaxPlayers.Value),
                    (Gender) cbGender.SelectedItem,
                    dtpStartDate.Value,
                    dtpEndDate.Value,
                    (TournamentSystem)Activator.CreateInstance((Type)cbTorunamentType.SelectedItem)
                    ); 
                _tournamentManager.CreateTournament(tournamentToBeCreated);
                MessageBox.Show($"Tournament {tournamentToBeCreated.Tittle} created");
                _allTournaments.ResetBindings();
            }
            catch (DuplicateEntryException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tournament tournament = (Tournament)lbTournaments.SelectedItem;
            if (tournament is not null)
            {
                DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete the tournament with id {tournament.Id}", 
                    "Delete", 
                    MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    _tournamentManager.DeleteTournament(tournament);
                    _allTournaments.ResetBindings();
                    MessageBox.Show("Tournament deleted");
                }
            }
            else
            {
                MessageBox.Show("Please select a tournamnet");
            }
           
        }
        
        private void UpdateRegisteredPlayersListbox(Tournament tournament)
        {
            lbRegisteredPlayers.Items.Clear();
            foreach (Customer c in tournament.RegisteredPlayers)
            {
                lbRegisteredPlayers.Items.Add(c.ToString());
            }
        }

        private void UpdateRoundsListbox(Tournament tournament)
        {
            lbRounds.Items.Clear();
            int counter = 1;
            List<Round> allRounds = tournament.AllTournamentRounds;
            foreach (Round r in allRounds)
            {
                r.Position = counter;
                counter++;
                lbRounds.Items.Add(r);
            }
        }

        private void UpdateMatchesListbox(Round round)
        {
            lbMatches.Items.Clear();
            foreach (Match m in round.Matches)
            {
                lbMatches.Items.Add(m);
                
            }
        }

        private void ClearMatchesListBox()
        {
            lbMatches.Items.Clear();
            nudFirstPlayerScore.Value = 0;
            nudSecondPlayerScore.Value = 0;
        }

        private void lbTournaments_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tournament tournament = (Tournament)lbTournaments.SelectedItem;
            ClearMatchesListBox();
            if (tournament is not null)
            {
                tbId.Text = tournament.Id.ToString();
                tbTitle.Text = tournament.Tittle;
                tbLocation.Text = tournament.Location;
                nudMinPlayers.Value = tournament.MinPlayers;
                nupMaxPlayers.Value = tournament.MaxPlayers;
                cbGender.SelectedItem = tournament.Gender;
                dtpStartDate.Value = tournament.StartDate;
                dtpEndDate.Value = tournament.EndDate;
                cbTorunamentType.SelectedItem = tournament.TournamentSystem.GetType();
                UpdateRegisteredPlayersListbox(tournament);
                UpdateRoundsListbox(tournament);

            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            tbId.Text = "";
            tbTitle.Text = "";
            tbLocation.Text = "";
            nudMinPlayers.Value = 0;
            nupMaxPlayers.Value = 0;
            cbGender.SelectedItem = "";
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tournament tournament = (Tournament)lbTournaments.SelectedItem;
            if (tournament is not null)
            {
                try
                {
                    Tournament tournamentToBeUpdated = new Tournament(
                        tournament.Id,
                        tbTitle.Text, tbLocation.Text,
                        Convert.ToInt32(nudMinPlayers.Value),
                        Convert.ToInt32(nupMaxPlayers.Value),
                        (Gender) cbGender.SelectedItem,
                        dtpStartDate.Value,
                        dtpEndDate.Value,
                        (TournamentSystem) Activator.CreateInstance((Type) cbTorunamentType.SelectedItem),
                        tournament.Status,
                        tournament.RegisteredPlayers
                    );
                    _tournamentManager.UpdateTournament(tournamentToBeUpdated);
                    _allTournaments.ResetBindings();
                    MessageBox.Show($"Tournament {tournamentToBeUpdated.Tittle} updated ");
                }
                catch (ArgumentException exception)
                {
                    MessageBox.Show(exception.Message);
                }
                catch (DuplicateEntryException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void lbRegisteredPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void lbRounds_SelectedIndexChanged(object sender, EventArgs e)
        {
            Round round = (Round)lbRounds.SelectedItem;
            ClearMatchesListBox();
            if (round is not null)
            {
                UpdateMatchesListbox(round);
            }
        }

        private void lbMatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            Match match = (Match)lbMatches.SelectedItem;
            if (match is not null)
            {
                nudFirstPlayerScore.Value = match.FirstPlayerScore;
                nudSecondPlayerScore.Value = match.SecondPlayerScore;
            }
        }

        private void btSaveScore_Click(object sender, EventArgs e)
        {
            Tournament tournament = (Tournament)lbTournaments.SelectedItem;
            try
            {
                _scheduleManager.SetPlayerScores(tournament, (Match)lbMatches.SelectedItem, Convert.ToInt32(nudFirstPlayerScore.Value), Convert.ToInt32(nudSecondPlayerScore.Value));
                MessageBox.Show("Score saved");
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btStartTournament_Click(object sender, EventArgs e)
        {
            Tournament tournament = (Tournament)lbTournaments.SelectedItem;
            if (tournament is not null)
            {
                try
                {
                    _tournamentManager.StartTournament(tournament);
                    UpdateRoundsListbox(tournament);
                    MessageBox.Show($"Tournament {tournament.Tittle} started");
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
