// ------------------------------------------------------------------------------------
// MigrationsHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using netca.Application.Common.Models;
using netca.Infrastructure.Persistence;
using Serilog;

namespace netca.Api.Handlers;

/// <summary>
/// MigrationsHandler
/// </summary>
public static class MigrationsHandler
{
    /// <summary>
    /// ApplyMigration
    /// </summary>
    /// <param name="app"></param>
    /// <param name="appSetting"></param>
    /// <returns></returns>
    public static async Task ApplyMigration(IApplicationBuilder app, AppSetting appSetting)
    {
        if (!appSetting.DatabaseSettings.Migrations && !appSetting.DatabaseSettings.SeedData)
            return;

        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        var logger = Log.ForContext(typeof(MigrationsHandler));

        try
        {
            logger.Warning("Migrating Database");
            if (serviceScope != null)
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (appSetting.DatabaseSettings.Migrations)
                {
                    await context.Database.MigrateAsync();
                }

                if (appSetting.DatabaseSettings.SeedData)
                {
                    logger.Warning("Seeding Database");
                    await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred while migrating the database");
        }
    }
}

/// <summary>
/// UseMigrationsHandlerExtensions
/// </summary>
public static class UseMigrationsHandlerExtensions
{
    /// <summary>
    /// UseMigrationsHandler
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="appSetting"></param>
    public static void UseMigrationsHandler(this IApplicationBuilder builder, AppSetting appSetting)
    {
        MigrationsHandler.ApplyMigration(builder, appSetting).ConfigureAwait(false);
    }
}