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
    /// <typeparam name="T">The type of the controller class</typeparam>
    [ApiController]
    public abstract class ApiControllerBase<T> : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// Protected variable to perform logging.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Gets protected variable to encapsulate request/response and publishing interaction patterns.
        /// </summary>
        /// <returns></returns>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase{T}"/> class.
        /// </summary>
        /// <param name="logger">Set Logger to perform logging</param>
        protected ApiControllerBase(ILogger<T> logger)
        {
            Logger = logger;
        }
    }
}