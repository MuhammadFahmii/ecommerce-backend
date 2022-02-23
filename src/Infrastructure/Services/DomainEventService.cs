// ------------------------------------------------------------------------------------
// DomainEventService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Domain.Common;

namespace netca.Infrastructure.Services;

/// <summary>
/// DomainEventService
/// </summary>
public class DomainEventService : IDomainEventService
{
    private readonly ILogger<DomainEventService> _logger;
    private readonly IPublisher _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Publish
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    public async Task Publish(DomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing domain event. Event - {Event}", domainEvent.GetType().Name);
        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    /// <summary>
    /// GetNotificationCorrespondingToDomainEvent
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}