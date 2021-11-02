// ------------------------------------------------------------------------------------
// ApiExceptionFilterAttribute.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace netca.Api.Filters
{
    /// <summary>
    /// ApiExceptionFilterAttribute
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionFilterAttribute"/> class.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApiExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiExceptionFilterAttribute>();
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(OperationCanceledException), HandleOperationCancelledException }
            };
        }

        /// <summary>
        /// OnException
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
            }
        }

        private void HandleOperationCancelledException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = 499,
                Title = "Client Closed Request",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            };

            _logger.LogWarning("Client Closed Request");

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            context.ExceptionHandled = true;
        }
    }
}
