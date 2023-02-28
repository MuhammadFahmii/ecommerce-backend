// ------------------------------------------------------------------------------------
// CreateTodoItemCommandValidator.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ecommerce.Application.Common.Interfaces;

namespace ecommerce.Application.TodoItems.Commands.CreateTodoItem;

/// <summary>
/// CreateTodoItemCommandValidator
/// </summary>
public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    private readonly IApplicationDbContext _context;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoItemCommandValidator"/> class.
    /// </summary>
    /// <param name="context"></param>
    public CreateTodoItemCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Id)
            .NotEmpty().NotEmpty();
        RuleFor(v => v.ListId)
            .NotEmpty().NotEmpty()
            .MustAsync(BeExistTodoList).WithMessage("The todolist not exists.");
        RuleFor(x => x.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
    
    private async Task<bool> BeExistTodoList(Guid id, CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .AnyAsync(l => l.Id == id, cancellationToken);
    }
}