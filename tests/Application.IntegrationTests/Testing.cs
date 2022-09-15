// ------------------------------------------------------------------------------------
// Testing.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    /// <summary>
    /// ScopeFactory
    /// </summary>
    public static IServiceScopeFactory? ScopeFactory { get; private set; }
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
            _factory = new CustomWebApplicationFactory();
            ScopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            _configuration = _factory.Services.GetRequiredService<IConfiguration>();

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
    /// Find
    /// </summary>
    /// <param name="keyValues"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static  TEntity? Find<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = ScopeFactory?.CreateScope();

        var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return  context?.Find<TEntity>(keyValues);
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