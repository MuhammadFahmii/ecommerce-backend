// ------------------------------------------------------------------------------------
// CreateTodoItemCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using MediatR;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;
using netca.Domain.Events;

namespace netca.Application.TodoItems.Commands.CreateTodoItem;

/// <summary>
/// CreateTodoItemCommand
/// </summary>
public class CreateTodoItemCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets or sets listId
    /// </summary>
    [Required]
    public Guid ListId { get; set; }

    /// <summary>
    /// Gets or sets title
    /// </summary>
    [Required]
    public string? Title { get; set; }
}

/// <summary>
/// CreateTodoItemCommandHandler
/// </summary>
public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserAuthorizationService _userAuthorizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoItemCommandHandler"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userAuthorizationService"></param>
    public CreateTodoItemCommandHandler(IApplicationDbContext context, IUserAuthorizationService userAuthorizationService)
    {
        _context = context;
        _userAuthorizationService = userAuthorizationService;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Unit> Handle(
        CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false,
            CreatedBy = _userAuthorizationService.GetAuthorizedUser().UserId
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));
        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}