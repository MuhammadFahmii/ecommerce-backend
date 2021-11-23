// ------------------------------------------------------------------------------------
// UpdateTodoItemDetailCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
        /// Gets or sets id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets listId
        /// </summary>
        public Guid ListId { get; set; }

        /// <summary>
        /// Gets or sets priority
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTodoItemDetailCommandHandler"/> class.
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
        /// <exception cref="NotFoundException">Exception</exception>
        public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoItems!.FindAsync(new object[] { request.Id }, cancellationToken);

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
