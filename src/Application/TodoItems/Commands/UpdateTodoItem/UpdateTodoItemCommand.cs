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
        /// Gets or sets id
        /// </summary>
        [BindRequired]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets title
        /// </summary>
        [BindRequired]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether done
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
        /// Initializes a new instance of the <see cref="UpdateTodoItemCommandHandler"/> class.
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
        /// <exception cref="NotFoundException">Exception</exception>
        public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoItems!.FindAsync(new object[] { request.Id }, cancellationToken);

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
