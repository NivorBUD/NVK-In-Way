using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NvkInWayWebApi.Domain.RepositoriesContract;
using NvkInWayWebApi.Persistence.Repositories;

namespace NvkInWayWebApi.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services,
            IConfiguration configuration)
        {
            //переделать под AppSettings!
            var connectionString = configuration["ConnectionString"];

            services.AddDbContext<DbContext, NvkInWayContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();

            return services;
        }
    }
}
