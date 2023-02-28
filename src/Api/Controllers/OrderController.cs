using MediatR;
using Microsoft.AspNetCore.Mvc;
using ecommerce.Api.Filters;
using ecommerce.Application.Common.Extensions;
using ecommerce.Application.Common.Models;
using ecommerce.Application.Orders.Commands;
using ecommerce.Application.Orders.Queries;
using ecommerce.Application.TodoLists.Commands.CreateTodoList;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ecommerce.Api.Controllers;

/// <summary>
/// Represent Restful Order Controller
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
[ServiceFilter(typeof(ApiAuthenticationFilterAttribute))]
public class OrderController : ApiControllerBase
{
    /// <summary>
    /// Init new order controller
    /// </summary>
    public OrderController()
    {
       
    }

    /// <summary>
    /// Create order
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, typeof(FileResult),
        Description = "Successfully to create order")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Unit), Description = Constants.ApiErrorDescription.BadRequest)]
    [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(Unit),
        Description = Constants.ApiErrorDescription.Unauthorized)]
    [SwaggerResponse(HttpStatusCode.Forbidden, typeof(Unit), Description = Constants.ApiErrorDescription.Forbidden)]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Unit),
        Description = Constants.ApiErrorDescription.InternalServerError)]
    public async Task<Unit> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        return await Mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Get order based on id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(FileResult),
        Description = "Successfully to get order")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Unit), Description = Constants.ApiErrorDescription.BadRequest)]
    [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(Unit),
        Description = Constants.ApiErrorDescription.Unauthorized)]
    [SwaggerResponse(HttpStatusCode.Forbidden, typeof(Unit), Description = Constants.ApiErrorDescription.Forbidden)]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Unit),
        Description = Constants.ApiErrorDescription.InternalServerError)]
    public async Task<DocumentRootJson<OrdersVm>> GetOrderByID(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetOrderByIDQuery { Id= id };

        return await Mediator.Send(command, cancellationToken);
    }
}