// ------------------------------------------------------------------------------------
// ApiDevelopmentFilterAttribute.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Extensions;

namespace netca.Api.Filters;

/// <summary>
/// ApiDevelopmentFilterAttribute
/// </summary>
public class ApiDevelopmentFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// OnActionExecutionAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiDevelopmentFilterAttribute>>();
        var environment = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        var actionDescriptor = context.ActionDescriptor;
        var ctrl = actionDescriptor.RouteValues["controller"].NullSafeToLower();
        var action = actionDescriptor.RouteValues["action"].NullSafeToLower();
        var env = environment.IsProduction();

        logger.LogDebug("Accessing {Ctrl} {Action}", ctrl, action);

        if (!env)
            await next();
        else
            context.Result = new NotFoundResult();
    }
}
