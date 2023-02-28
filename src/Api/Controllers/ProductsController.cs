using MediatR;
using Microsoft.AspNetCore.Mvc;
using ecommerce.Application.Common.Models;
using ecommerce.Application.Products.Commands;
using NSwag.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ecommerce.Api.Controllers;

/// <summary>
///
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ApiControllerBase
{
    /// <summary>
    ///
    /// </summary>
    public ProductsController()
    {
    }

    /// <summary>
    /// Create Product
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, typeof(FileResult),
        Description = "Successfully to create product")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Unit), Description = Constants.ApiErrorDescription.BadRequest)]
    [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(Unit),
        Description = Constants.ApiErrorDescription.Unauthorized)]
    [SwaggerResponse(HttpStatusCode.Forbidden, typeof(Unit), Description = Constants.ApiErrorDescription.Forbidden)]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Unit),
        Description = Constants.ApiErrorDescription.InternalServerError)]
    public async Task<Unit> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        return await Mediator.Send(command, cancellationToken);
    }
}