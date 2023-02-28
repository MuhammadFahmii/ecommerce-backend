// ------------------------------------------------------------------------------------
// CreateTodoListCommandValidator.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ecommerce.Application.Common.Interfaces;

namespace ecommerce.Application.TodoLists.Commands.CreateTodoList
{
    /// <summary>
    /// CreateTodoListCommandValidator
    /// </summary>
    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTodoListCommandValidator"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CreateTodoListCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Id)
                .NotEmpty().NotEmpty();
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
                .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.TodoLists!
                .AllAsync(l => l.Title != title, cancellationToken);
        }
    }
}
