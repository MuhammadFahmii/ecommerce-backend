// ------------------------------------------------------------------------------------
// UpdateTodoItemDetailCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;
using netca.Domain.Enums;

namespace netca.Application.TodoItems.Commands.UpdateTodoItemDetail
{
    /// <summary>
    /// UpdateTodoItemDetailCommand
    /// </summary>
    public class UpdateTodoItemDetailCommand : IRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// ListId
        /// </summary>
        public Guid ListId { get; set; }
        
        /// <summary>
        /// Priority
        /// </summary>
        public PriorityLevel Priority { get; set; }
        
        /// <summary>
        /// Note
        /// </summary>
        public string Note { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
    {
        private readonly IApplicationDbContext _context;
        
        /// <summary>
        /// UpdateTodoItemDetailCommandHandler
        /// </summary>
        /// <param name="context"></param>
        public UpdateTodoItemDetailCommandHandler(IApplicationDbContext context)
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
        public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoItems.FindAsync(new object[]{request.Id}, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            entity.ListId = request.ListId;
            entity.Priority = request.Priority;
            entity.Note = request.Note;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
