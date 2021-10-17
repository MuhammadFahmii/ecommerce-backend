// ------------------------------------------------------------------------------------
// DependencyInjection.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using netca.Application;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Application.TodoLists.Queries.GetTodos;
using netca.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using netca.Infrastructure.Files;
using netca.Infrastructure.Jobs;
using netca.Infrastructure.Persistence;
using netca.Infrastructure.Services;
using netca.Infrastructure.Services.Cache;
using Quartz;

namespace netca.Infrastructure
{
    /// <summary>
    /// DependencyInjection
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// AddInfrastructure
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        public static void AddInfrastructure(this IServiceCollection services,
            IWebHostEnvironment environment, AppSetting appSetting)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    appSetting.ConnectionStrings.DefaultConnection,
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.CommandTimeout(appSetting.DatabaseSettings.CommandTimeout);
                        b.EnableRetryOnFailure(appSetting.DatabaseSettings.MaxRetryCount,
                            TimeSpan.FromSeconds(appSetting.DatabaseSettings.MaxRetryDelay), null);

                        if (!environment.IsProduction())
                        {
                            options.EnableSensitiveDataLogging();
                        }
                    }),
                ServiceLifetime.Transient);
            services.AddTransient<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
            services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandlerService>();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddHostedService<LifetimeEventsHostedService>();
             
            var md  = CreateServiceProvider(services).GetService<ISender>();
            var xy = md.Send(new GetTodosQuery());
            Console.WriteLine("xxxx"+xy.Id);
            var x = md.Send(new GetWeatherForecastsQuery()).Result;
            foreach (var xx in x)
            {
                Console.WriteLine(xx.Date);
                Console.WriteLine(xx.Summary);
            }
            if (!appSetting.BackgroundJob.IsEnable) return;
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                q.AddJobAndTrigger<HelloWorldJob>(appSetting);
            });

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
        }
        
        private static ServiceProvider CreateServiceProvider(IServiceCollection services){
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}