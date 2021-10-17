// ------------------------------------------------------------------------------------
// UpdateTodoList.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;

namespace netca.Application.TodoLists.Commands.UpdateTodoList
{
    /// <summary>
    /// UpdateTodoList
    /// </summary>
    public class UpdateTodoListCommand : IRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        [BindRequired]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Title
        /// </summary>
        [BindRequired]
        public string Title { get; set; }
    }
    
    /// <summary>
    /// UpdateTodoListCommandHandler
    /// </summary>
    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
    {
        private readonly IApplicationDbContext _context;
        /// <summary>
        /// UpdateTodoListCommandHandler
        /// </summary>
        /// <param name="context"></param>
        public UpdateTodoListCommandHandler(IApplicationDbContext context)
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
        public async Task<Unit> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoLists.FindAsync(new object[]{request.Id}, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            entity.Title = request.Title;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
