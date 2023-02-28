// ------------------------------------------------------------------------------------
// TodoItemCreatedEventHandler.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using Microsoft.Extensions.Logging;
using ecommerce.Domain.Events;

namespace ecommerce.Application.TodoItems.EventHandlers;

/// <summary>
/// TodoItemCreatedEventHandler
/// </summary>
public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger;
    
    /// <summary>
    /// TodoItemCreatedEventHandler
    /// </summary>
    /// <param name="logger"></param>
    public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ecommerce Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}