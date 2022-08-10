// ------------------------------------------------------------------------------------
// DeleteTodoListCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using MediatR;
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
        /// Gets or sets id
        /// </summary>
        [Required]
        public Guid Id { get; set; }
    }

    /// <summary>
    /// DeleteTodoListCommandHandler
    /// </summary>
    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTodoListCommandHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userAuthorizationService"></param>
        /// <param name="dateTime"></param>
        public DeleteTodoListCommandHandler(IApplicationDbContext context, IUserAuthorizationService userAuthorizationService, IDateTime dateTime)
        {
            _context = context;
            _userAuthorizationService = userAuthorizationService;
            _dateTime = dateTime;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Exception</exception>
        public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoLists
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }
            
            entity.UpdatedBy = _userAuthorizationService.GetAuthorizedUser().UserId;
            entity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
