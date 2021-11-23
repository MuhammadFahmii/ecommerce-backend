// ------------------------------------------------------------------------------------
// CreateTodoItemCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Vms;
using netca.Domain.Entities;
using netca.Domain.Events;

namespace netca.Application.TodoItems.Commands.CreateTodoItem
{
    /// <summary>
    /// CreateTodoItemCommand
    /// </summary>
    public class CreateTodoItemCommand : IRequest<DocumentRootJson<CreatedVm>>
    {
        /// <summary>
        /// Gets or sets listId
        /// </summary>
        [BindRequired]
        public Guid ListId { get; set; }

        /// <summary>
        /// Gets or sets title
        /// </summary>
        [BindRequired]
        public string? Title { get; set; }
    }

    /// <summary>
    /// CreateTodoItemCommandHandler
    /// </summary>
    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, DocumentRootJson<CreatedVm>>
    {
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTodoItemCommandHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CreateTodoItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DocumentRootJson<CreatedVm>> Handle(
            CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new TodoItem
            {
                ListId = request.ListId,
                Title = request.Title,
                Done = false
            };

            entity.DomainEvents.Add(new TodoItemCreatedEvent(entity));

            _context.TodoItems!.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return JsonApiExtensions.ToJsonApi(new CreatedVm { Id = entity.Id });
        }
    }
}