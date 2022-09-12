// ------------------------------------------------------------------------------------
// MigrationsHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using netca.Application.Common.Models;
using netca.Infrastructure.Persistence;

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
    /// <param name="environment"></param>
    /// <param name="appSetting"></param>
    /// <returns></returns>
    public static async Task ApplyMigration(IApplicationBuilder app, IWebHostEnvironment environment, AppSetting appSetting)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        var initializer = serviceScope?.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        if (appSetting.DatabaseSettings.Migrations)
        {
            await initializer?.InitialiseAsync()!;
        }

        if (appSetting.DatabaseSettings.SeedData && environment.IsDevelopment())
        {
            await initializer?.SeedAsync()!;
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
    /// <param name="environment"></param>
    /// <param name="appSetting"></param>
    public static void UseMigrationsHandler(this IApplicationBuilder builder, IWebHostEnvironment environment,AppSetting appSetting)
    {
        MigrationsHandler.ApplyMigration(builder, environment, appSetting).ConfigureAwait(false);
    }
}
