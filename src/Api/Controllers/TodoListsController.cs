// ------------------------------------------------------------------------------------
// TodoListsController.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netca.Application.TodoLists.Queries.GetTodos;
using NSwag.Annotations;

namespace netca.Api.Controllers
{
    /// <summary>
    /// Represents RESTful of TodoListsController
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/todoLists")]
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
        /// Get Todos
        /// </summary>
        /// <param name="query">
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to user log cancel the operation
        /// </param>
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
    }
}
