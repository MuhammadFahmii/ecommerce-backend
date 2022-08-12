// ------------------------------------------------------------------------------------
// TodoItemCompletedEventHandler.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using Microsoft.Extensions.Logging;
using netca.Domain.Events;

namespace netca.Application.TodoItems.EventHandlers;

/// <summary>
/// TodoItemCompletedEventHandler
/// </summary>
public class TodoItemCompletedEventHandler : INotificationHandler<TodoItemCompletedEvent>
{
    private readonly ILogger<TodoItemCompletedEventHandler> _logger;
    
    /// <summary>
    /// TodoItemCompletedEventHandler
    /// </summary>
    /// <param name="logger"></param>
    public TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("netca Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}