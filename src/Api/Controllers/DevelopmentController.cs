// ------------------------------------------------------------------------------------
// DevelopmentController.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using netca.Api.Filters;
using netca.Application.Common.Extensions;
using netca.Application.Common.Models;
using netca.Application.Common.Vms;
using netca.Application.TodoLists.Queries.GetTodos;
using NSwag.Annotations;

namespace netca.Api.Controllers;

/// <summary>
/// Represents RESTful of DevelopmentController
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/development")]
[ServiceFilter(typeof(ApiDevelopmentFilterAttribute))]
public class DevelopmentController : ApiControllerBase
{
    /// <summary>
    /// get todos
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces(Constants.HeaderJsonVndApi)]
    [SwaggerResponse(HttpStatusCode.OK, typeof(DocumentRootJson<List<TodoListVm>>), Description = "Successfully to get todos")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Unit), Description = Constants.ApiErrorDescription.BadRequest)]
    [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(Unit),
        Description = Constants.ApiErrorDescription.Unauthorized)]
    [SwaggerResponse(HttpStatusCode.Forbidden, typeof(Unit), Description = Constants.ApiErrorDescription.Forbidden)]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Unit),
        Description = Constants.ApiErrorDescription.InternalServerError)]
    public async Task<DocumentRootJson<List<TodoListVm>>> GetAsync([FromQuery] GetTodosQuery query,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }
}