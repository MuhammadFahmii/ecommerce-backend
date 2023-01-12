// ------------------------------------------------------------------------------------
// CustomWebApplicationFactory.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Infrastructure.Persistence;

namespace netca.Application.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json", false, false)
                .AddJsonFile($"appsettings.Test.Local.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builderContext, services) =>
        {
            services.AddSingleton(Mock.Of<IWebHostEnvironment>(x => x.EnvironmentName == "Test" && x.ApplicationName == "netca.Api"));
            
            services
                .Remove<IUserAuthorizationService>()
                .AddTransient(provider => Mock.Of<IUserAuthorizationService>(s =>
                    s.GetAuthorizedUser() == MockData.GetAuthorizedUser()));
            
            var redis = new Mock<IRedisService>();
            redis.Setup(x => x.SaveSubAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true))
                .ReturnsAsync("KEYS");
            services.AddSingleton(_ => redis.Object);


            services
                .Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                    {
                        options.UseSqlServer(builderContext.Configuration.GetConnectionString("DefaultConnection"),
                            dbContextOptionsBuilder =>
                                dbContextOptionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly
                                    .FullName));
                    }
            );
        });
    }
}