using BLL.Managers;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;
using Modules.Tools;
using SynthesissAssignment.Tools;

namespace SynthesissAssignment
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        private static readonly IServiceProvider _provider = ServiceManager.Get();

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var userRepository = _provider.GetService<IUserRepository>();
            var tournamentRepository = _provider.GetService<ITournamentRepository>();
            var scheduleRepository = _provider.GetService<IScheduleRepository>();


            //finish injection after implementing gui :D

            ApplicationConfiguration.Initialize();
            Application.Run(new LogIn(tournamentRepository, userRepository, scheduleRepository));

        }
    }
}