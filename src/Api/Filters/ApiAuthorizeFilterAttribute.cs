// ------------------------------------------------------------------------------------
// ApiAuthorizeFilterAttribute.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;

namespace netca.Api.Filters
{
    /// <summary>
    /// ApiAuthorizeFilterAttribute
    /// </summary>
    public class ApiAuthorizeFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecutionAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiAuthorizeFilterAttribute>>();
            var appSetting = context.HttpContext.RequestServices.GetRequiredService<AppSetting>();
            var actionDescriptor = context.ActionDescriptor;
            var ctrl = actionDescriptor.RouteValues["controller"].ToLower();
            var action = actionDescriptor.RouteValues["action"].ToLower();
            var permission = $"{appSetting.AuthorizationServer.Service}:{context.HttpContext.Request.Method}:{ctrl}_{action}";
            try
            {
                var policy = GetPolicy(logger, appSetting, permission);
                if (policy is { IsCheck: true })
                {
                    var auth = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
                    logger.LogDebug($"Checking permission {policy.Name}");
                    var permissionChek=  auth.AuthorizeAsync(context.HttpContext.User, null, policy.Name).Result;
                    if (!permissionChek.Succeeded)
                    {
                        context.Result = new ForbidResult();  
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            catch (Exception e)
            {
                logger.LogWarning($"error Checking permission {e.Message}");
                context.Result = new ForbidResult();  
            }
        }
        private static Policy GetPolicy(ILogger logger, AppSetting appSetting, string policy)
        {
            logger.LogDebug($"Get Policy {policy} if exists");
            var policyList = appSetting.AuthorizationServer.Policy;
            return policyList.Count.Equals(0) ? null : policyList.FirstOrDefault(x => x.Name.ToLower().Equals(policy.ToLower()));
        }
    }
}
