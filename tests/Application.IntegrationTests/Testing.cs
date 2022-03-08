// ------------------------------------------------------------------------------------
// Testing.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using netca.Api;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Infrastructure.Persistence;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;

namespace netca.Application.IntegrationTests;

/// <summary>
/// Testing
/// </summary>>
[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot? _configuration;
    /// <summary>
    /// ScopeFactory
    /// </summary>
    public static IServiceScopeFactory? ScopeFactory { get; private set; }
    private static Mock<IUserAuthorizationService>? _auth;
    private static Mock<IRedisService>? _redis;
    private static Checkpoint? _checkpoint;

    /// <summary>
    /// RunBeforeAnyTests
    /// </summary>
    /// <exception cref="Exception"></exception>
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        try
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", false, true)
                .AddJsonFile($"appsettings.Test.Local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();
            startup.ConfigureServices(services);
            
            services.AddSingleton(Mock.Of<IWebHostEnvironment>(x => x.EnvironmentName == "Test" && x.ApplicationName == "netca.Api"));

            services.AddLogging();

            startup.ConfigureServices(services);

            var currentUserServiceDescriptor =
                services.FirstOrDefault(d => d.ServiceType == typeof(IUserAuthorizationService));

            if (currentUserServiceDescriptor != null) services.Remove(currentUserServiceDescriptor);

            _auth = new Mock<IUserAuthorizationService>();
            _auth.Setup(x => x.GetAuthorizedUser()).Returns(MockData.GetAuthorizedUser());
            services.AddTransient(_ => _auth.Object);

            _redis = new Mock<IRedisService>();
            _redis.Setup(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("KEYS");
            services.AddSingleton(_ => _redis.Object);

            ScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            
            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
            };
            EnsureDatabase();
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString(), e);
        }
    }
    
    /// <summary>
    /// ResetState
    /// </summary>
    public static async Task ResetState()
    {
        await _checkpoint?.Reset(_configuration.GetConnectionString("DefaultConnection"))!;
    }
    
    private static void EnsureDatabase()
    {
        using var scope = ScopeFactory?.CreateScope();

        var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context?.Database.Migrate();
    }

    
    /// <summary>
    /// SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = ScopeFactory?.CreateScope();
        var mediator = scope?.ServiceProvider.GetService<IMediator>();

        return await mediator?.Send(request)!;
    }
    
    /// <summary>
    /// AddAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = ScopeFactory?.CreateScope();
        var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context?.Add(entity);

        await context?.SaveChangesAsync()!;
    }
    
    /// <summary>
    /// CountAsync
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = ScopeFactory?.CreateScope();

        var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context?.Set<TEntity>().CountAsync()!;
    }
}