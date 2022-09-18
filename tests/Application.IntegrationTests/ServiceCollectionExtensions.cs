// ------------------------------------------------------------------------------------
// ServiceCollectionExtensions.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace netca.Application.IntegrationTests;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Remove
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static IServiceCollection Remove<TService>(this IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(TService));

        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }

        return services;
    }
}