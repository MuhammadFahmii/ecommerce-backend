// ------------------------------------------------------------------------------------
// UpdateTodoItemDetailCommandValidator.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ecommerce.Application.Common.Interfaces;

namespace ecommerce.Application.TodoItems.Commands.UpdateTodoItemDetail;

/// <summary>
/// UpdateTodoItemDetailCommandValidator
/// </summary>
public class UpdateTodoItemDetailCommandValidator : AbstractValidator<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTodoItemDetailCommandValidator"/> class.
    /// </summary>
    /// <param name="context"></param>
    public UpdateTodoItemDetailCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Id)
            .NotNull().NotEmpty();
        RuleFor(v => v.ListId)
            .NotNull().NotEmpty()
            .MustAsync(BeExistTodoList).WithMessage("The todolist not exists.");
        RuleFor(v => v.Priority)
            .Must(x => x >= 0);
    }
    
    private async Task<bool> BeExistTodoList(Guid id, CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .AnyAsync(l => l.Id == id, cancellationToken);
    }
}