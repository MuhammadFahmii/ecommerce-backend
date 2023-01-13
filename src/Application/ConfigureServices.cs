// ------------------------------------------------------------------------------------
// ConfigureServices.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using netca.Application.Common.Behaviours;
using netca.Application.Common.Interfaces;
using Scrutor;

namespace netca.Application;

/// <summary>
/// ConfigureServices
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// AddApplicationServices
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(ConfigureServices).Assembly;

        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(assembly);
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RateLimitPolicyBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(BulkheadPolicyBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(CircuitBreakerPolicyBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TimeoutPolicyBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FallbackBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryPolicyBehavior<,>));

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICachePolicy<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IFallbackHandler<,>)))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}