// ------------------------------------------------------------------------------------
// TodoListsController.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using netca.Api.Filters;
using netca.Application.Common.Extensions;
using netca.Application.Common.Models;
using netca.Application.Common.Vms;
using netca.Application.TodoLists.Queries.ExportTodos;
using netca.Application.TodoLists.Queries.GetTodos;
using NSwag.Annotations;

namespace netca.Api.Controllers;

/// <summary>
/// Represents RESTful of TodoListsController
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/todoLists")]
[ServiceFilter(typeof(ApiAuthorizeFilterAttribute))]
public class TodoListsController : ApiControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListsController"/> class.
    /// </summary>
    public TodoListsController()
    {
    }

    /// <summary>
    /// get todos
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces(Constants.HeaderJson)]
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

    /// <summary>
    /// Get Todos csv
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Produces(Constants.HeaderTextCsv)]
    [SwaggerResponse(HttpStatusCode.OK, typeof(FileResult),
        Description = "Successfully to get todos csv")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Unit), Description = Constants.ApiErrorDescription.BadRequest)]
    [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(Unit),
        Description = Constants.ApiErrorDescription.Unauthorized)]
    [SwaggerResponse(HttpStatusCode.Forbidden, typeof(Unit), Description = Constants.ApiErrorDescription.Forbidden)]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Unit),
        Description = Constants.ApiErrorDescription.InternalServerError)]
    public async Task<FileResult> GetCsvAsync(Guid id, CancellationToken cancellationToken)
    {
        var vm = await Mediator.Send(new ExportTodosQuery { ListId = id }, cancellationToken);

        return File(vm.Content!, vm.ContentType!, vm.FileName);
    }
}