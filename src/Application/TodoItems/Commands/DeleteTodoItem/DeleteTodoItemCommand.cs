// ------------------------------------------------------------------------------------
// DeleteTodoItemCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;

namespace netca.Application.TodoItems.Commands.DeleteTodoItem;

/// <summary>
/// DeleteTodoItemCommand
/// </summary>
public class DeleteTodoItemCommand : IRequest
{
    /// <summary>
    /// Gets or sets id
    /// </summary>
    [BindRequired]
    public Guid Id { get; set; }
}

/// <summary>
/// DeleteTodoItemCommandHandler
/// </summary>
public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTodoItemCommandHandler"/> class.
    /// DeleteTodoItemCommandHandler
    /// </summary>
    /// <param name="context"></param>
    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
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
        var entity = await _context.TodoItems!.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        _context.TodoItems.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}