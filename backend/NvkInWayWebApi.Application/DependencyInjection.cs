using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NvkInWayWebApi.Application.Interfaces;
using NvkInWayWebApi.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NvkInWayWebApi.Application.Common;

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

        public static IServiceCollection AddControllerWithInlineValidation(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Регистрация валидаторов из текущей сборки
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .SelectMany(e => e.Value.Errors.Select(x => new ValidationErrorResponse(e.Key, x.ErrorMessage)))
                            .ToList();

                        return new BadRequestObjectResult(
                            new MyResponseMessage("One or more validation errors occurred.", errors));
                    };
                })
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            return services;
        }
    }
}
