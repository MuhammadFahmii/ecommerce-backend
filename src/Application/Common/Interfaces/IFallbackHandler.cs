// ------------------------------------------------------------------------------------
// IFallbackHandler.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;

namespace ecommerce.Application.Common.Interfaces;

/// <summary>
/// Interface of fallback policy implementation.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IFallbackHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// HandleFallback
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<TResponse> HandleFallback(TRequest request, CancellationToken cancellationToken);
}