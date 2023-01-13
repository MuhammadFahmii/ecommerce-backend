using Microsoft.AspNetCore.Mvc;

namespace netca.Api.Controllers;

/// <summary>
/// Represents RESTful of ApitoApi
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/apitoapi")]
public class ApitoApiController : ApiControllerBase
{
}
