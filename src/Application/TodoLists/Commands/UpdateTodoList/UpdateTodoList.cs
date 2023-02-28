// ------------------------------------------------------------------------------------
// UpdateTodoList.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using MediatR;
using ecommerce.Application.Common.Exceptions;
using ecommerce.Application.Common.Interfaces;
using ecommerce.Domain.Entities;

namespace ecommerce.Application.TodoLists.Commands.UpdateTodoList
{
    /// <summary>
    /// UpdateTodoList
    /// </summary>
    public class UpdateTodoListCommand : IRequest
    {
        /// <summary>
        /// Gets or sets id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets title
        /// </summary>
        [Required]
        public string? Title { get; set; }
    }

    /// <summary>
    /// UpdateTodoListCommandHandler
    /// </summary>
    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAuthorizationService _userAuthorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTodoListCommandHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userAuthorizationService"></param>
        public UpdateTodoListCommandHandler(IApplicationDbContext context, IUserAuthorizationService userAuthorizationService)
        {
            _context = context;
            _userAuthorizationService = userAuthorizationService;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Exception</exception>
        public async Task<Unit> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TodoLists.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            entity.Title = request.Title;
            entity.UpdatedBy = _userAuthorizationService.GetAuthorizedUser().UserId;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
