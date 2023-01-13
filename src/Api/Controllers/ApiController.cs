// ------------------------------------------------------------------------------------
// ApiController.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace netca.Api.Controllers;

/// <summary>
/// Base class for object controllers.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator? _mediator;

    /// <summary>
    /// Gets protected variable to encapsulate request/response and publishing interaction patterns.
    /// </summary>
    /// <returns></returns>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
