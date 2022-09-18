// ------------------------------------------------------------------------------------
// UpdateTodoItemDetailCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using MediatR;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;
using netca.Domain.Enums;

namespace netca.Application.TodoItems.Commands.UpdateTodoItemDetail;

/// <summary>
/// UpdateTodoItemDetailCommand
/// </summary>
public class UpdateTodoItemDetailCommand : IRequest
{
    /// <summary>
    /// Gets or sets id
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets listId
    /// </summary>
    [Required]
    public Guid ListId { get; set; }

    /// <summary>
    /// Gets or sets priority
    /// </summary>
    [Required]
    public PriorityLevel Priority { get; set; }

    /// <summary>
    /// Gets or sets note
    /// </summary>
    public string? Note { get; set; }
}

/// <summary>
/// UpdateTodoItemDetailCommandHandler
/// </summary>
public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserAuthorizationService _userAuthorizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTodoItemDetailCommandHandler"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userAuthorizationService"></param>
    public UpdateTodoItemDetailCommandHandler(IApplicationDbContext context, IUserAuthorizationService userAuthorizationService)
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
    /// <exception cref="NotFoundException">Exception</exception>
    public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;
        entity.UpdatedBy = _userAuthorizationService.GetAuthorizedUser().UserId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}