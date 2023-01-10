using System.Security.Authentication;
using BLL.Managers;
using DAL.Repositories;
using Modules.Entities;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;
using Modules.Tools;

namespace SynthesissAssignment
{
    public partial class LogIn : Form
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly UserManager _userManager;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public LogIn(ITournamentRepository tournamentRepository, IUserRepository userRepository, IScheduleRepository scheduleRepository)
        {
            InitializeComponent();
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;
            _userManager = new UserManager(_userRepository);
            _authenticationManager = new AuthenticationManager(_userManager);
            _scheduleRepository = scheduleRepository;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_authenticationManager.AuthenticateStaff(tbUsername.Text, tbPassword.Text))
                {
                    StaffForm form = new StaffForm(_tournamentRepository, _userRepository, _scheduleRepository);
                    this.Hide();
                    form.ShowDialog();
                    tbUsername.Text = "";
                    tbPassword.Text = "";
                    this.Show();
                }

            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}