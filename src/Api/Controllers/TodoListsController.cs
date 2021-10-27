// ------------------------------------------------------------------------------------
// TodoListsController.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netca.Api.Filters;
using netca.Application.TodoLists.Queries.ExportTodos;
using netca.Application.TodoLists.Queries.GetTodos;
using NSwag.Annotations;

namespace netca.Api.Controllers
{
    /// <summary>
    /// Represents RESTful of TodoListsController
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/todoLists")]
    [ServiceFilter(typeof(ApiAuthorizeFilterAttribute))]
    public class TodoListsController : ApiControllerBase<TodoListsController>
    {
        /// <summary>
        /// TodoListsController
        /// </summary>
        /// <param name="logger"></param>
        public TodoListsController(ILogger<TodoListsController> logger) : base(logger)
        {
        }
        
        
        
        /// <summary>
        /// get todos
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(DocumentRootJson<TodosVm>), Description = "Successfully to get todos")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Unit), Description = "BadRequest")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(Unit), Description = "Unauthorized")]
        [SwaggerResponse(HttpStatusCode.Forbidden, typeof(Unit), Description = "Forbidden")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Unit), Description = "Internal Server Error")]
        public async Task<DocumentRootJson<TodosVm>> GetAsync([FromQuery] GetTodosQuery query, CancellationToken cancellationToken)
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
        [Produces("text/csv")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(DocumentRootJson<TodosVm>), Description = "Successfully to get todos csv")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Unit), Description = "BadRequest")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(Unit), Description = "Unauthorized")]
        [SwaggerResponse(HttpStatusCode.Forbidden, typeof(Unit), Description = "Forbidden")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Unit), Description = "Internal Server Error")]
        public async Task<FileResult> GetCsvAsync(Guid id, CancellationToken cancellationToken)
        {
            var vm = await Mediator.Send(new ExportTodosQuery { ListId = id }, cancellationToken);

            return File(vm.Content, vm.ContentType, vm.FileName);
        }
    }
}
