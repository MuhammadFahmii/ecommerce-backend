// ------------------------------------------------------------------------------------
// ApiController.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace netca.Api.Controllers
{ 
    /// <summary>
    /// ApiController
    /// </summary>
    [ApiController]
    public abstract class ApiControllerBase<T> : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Mediator for command
        /// </summary>
        /// <returns></returns>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger"></param>
        protected ApiControllerBase(ILogger<T> logger)
        {
            Logger = logger;
        }
    }
}
