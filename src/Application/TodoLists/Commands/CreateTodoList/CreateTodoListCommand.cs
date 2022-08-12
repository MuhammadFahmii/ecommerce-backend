// ------------------------------------------------------------------------------------
// CreateTodoListCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using MediatR;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;

namespace netca.Application.TodoLists.Commands.CreateTodoList
{
    /// <summary>
    /// CreateTodoListCommand
    /// </summary>
    public class CreateTodoListCommand : IRequest<Unit>
    {
        /// <summary>
        /// Gets or sets title
        /// </summary>
        [Required]
        public string? Title { get; set; }
    }

    /// <summary>
    /// CreateTodoListCommandHandler
    /// </summary>
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAuthorizationService _userAuthorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTodoListCommandHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userAuthorizationService"></param>
        public CreateTodoListCommandHandler(IApplicationDbContext context, IUserAuthorizationService userAuthorizationService)
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
        public async Task<Unit> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = new TodoList
            {
                Title = request.Title,
                CreatedBy = _userAuthorizationService.GetAuthorizedUser().UserId
            };

            _context.TodoLists.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
