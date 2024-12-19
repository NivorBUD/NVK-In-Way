using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Application.Interfaces;
using NvkInWayWebApi.Application.Services;

namespace NvkInWayWebApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDriverService, DriverService>();

            //services.AddScoped<IPassengerService, PassengerService>();

            return services;
        }
    }
}
