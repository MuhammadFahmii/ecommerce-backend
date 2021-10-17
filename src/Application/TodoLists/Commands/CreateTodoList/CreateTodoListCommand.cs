// ------------------------------------------------------------------------------------
// CreateTodoListCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Vms;
using netca.Domain.Entities;

namespace netca.Application.TodoLists.Commands.CreateTodoList
{
    /// <summary>
    /// CreateTodoListCommand
    /// </summary>
    public class CreateTodoListCommand : IRequest<DocumentRootJson<CreatedVm>>
    {
        /// <summary>
        /// Title
        /// </summary>
        [BindRequired]
        public string Title { get; set; }
    }
    
    /// <summary>
    /// CreateTodoListCommandHandler
    /// </summary>
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, DocumentRootJson<CreatedVm>>
    {
        private readonly IApplicationDbContext _context;
        
        /// <summary>
        /// CreateTodoListCommandHandler
        /// </summary>
        /// <param name="context"></param>
        public CreateTodoListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DocumentRootJson<CreatedVm>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = new TodoList
            {
                Title = request.Title
            };

            _context.TodoLists.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return JsonApiExtensions.ToJsonApi( new CreatedVm{Id = entity.Id});
        }
    }
}
