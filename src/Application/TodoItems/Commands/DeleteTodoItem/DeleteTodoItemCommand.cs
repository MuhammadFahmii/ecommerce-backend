// ------------------------------------------------------------------------------------
// DeleteTodoItemCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using MediatR;
using ecommerce.Application.Common.Exceptions;
using ecommerce.Application.Common.Interfaces;
using ecommerce.Domain.Entities;

namespace ecommerce.Application.TodoItems.Commands.DeleteTodoItem;

/// <summary>
/// DeleteTodoItemCommand
/// </summary>
public class DeleteTodoItemCommand : IRequest
{
    /// <summary>
    /// Gets or sets id
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}

/// <summary>
/// DeleteTodoItemCommandHandler
/// </summary>
public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserAuthorizationService _userAuthorizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTodoItemCommandHandler"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userAuthorizationService"></param>
    public DeleteTodoItemCommandHandler(IApplicationDbContext context, IUserAuthorizationService userAuthorizationService)
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
    public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        entity.UpdatedBy = _userAuthorizationService.GetAuthorizedUser().UserId;
        entity.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}