// ------------------------------------------------------------------------------------
// UpdateTodoItemCommand.cs  2021
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

namespace netca.Application.TodoItems.Commands.UpdateTodoItem
{
    /// <summary>
    /// UpdateTodoItemCommand
    /// </summary>
    public class UpdateTodoItemCommand : IRequest
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
        
        /// <summary>
        /// Done
        /// </summary>\
        [BindRequired]
        public bool Done { get; set; }
    }
    
    /// <summary>
    /// UpdateTodoItemCommandHandler
    /// </summary>
    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
    {
        private readonly IApplicationDbContext _context;
        /// <summary>
        /// UpdateTodoItemCommandHandler
        /// </summary>
        /// <param name="context"></param>
        public UpdateTodoItemCommandHandler(IApplicationDbContext context)
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
        public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoItems.FindAsync(new object[]{request.Id}, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            entity.Title = request.Title;
            entity.Done = request.Done;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
