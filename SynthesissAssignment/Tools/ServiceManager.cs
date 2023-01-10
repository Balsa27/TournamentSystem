using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modules.Interfaces.Manager;
using Modules.Interfaces.Repository;
using BLL.Managers;
using DAL.Repositories;

namespace SynthesissAssignment.Tools
{
    public class ServiceManager
    {
        //This is done because if we have to change an implementation of a class, we only have to change it in one place.
        public static IServiceProvider Get() //returns an object that implements IServiceProvider
        {
            //Service is a long-running application that can be started automatically when your system is started.You can pause and restart the service.
            var services = new ServiceCollection(); //list which contains all the services


            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ITournamentRepository, TournamentRepository>();
            services.AddSingleton<IScheduleRepository, ScheduleRepository>();
            //services.AddSingleton<IRoundRepository, RoundRepository>();

            return services.BuildServiceProvider(); //returns a provider with configurations from above
        }
    }
}