// ------------------------------------------------------------------------------------
// DeleteTodoListCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;

namespace netca.Application.TodoLists.Commands.DeleteTodoList
{
    /// <summary>
    /// DeleteTodoListCommand
    /// </summary>
    public class DeleteTodoListCommand : IRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        [BindRequired]
        public Guid Id { get; set; }
    }
    
    /// <summary>
    /// DeleteTodoListCommandHandler
    /// </summary>
    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
    {
        private readonly IApplicationDbContext _context;
        
        /// <summary>
        /// DeleteTodoListCommandHandler
        /// </summary>
        /// <param name="context"></param>
        public DeleteTodoListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoLists
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            _context.TodoLists.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
