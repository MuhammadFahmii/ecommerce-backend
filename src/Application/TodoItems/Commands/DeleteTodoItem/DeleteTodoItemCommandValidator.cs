// ------------------------------------------------------------------------------------
// DeleteTodoItemCommandValidator.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using FluentValidation;

namespace netca.Application.TodoItems.Commands.DeleteTodoItem;

/// <summary>
/// DeleteTodoItemCommandValidator
/// </summary>
public class DeleteTodoItemCommandValidator : AbstractValidator<DeleteTodoItemCommand>
{
    /// <summary>
    /// DeleteTodoItemCommandValidator
    /// </summary>
    public DeleteTodoItemCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull().NotEmpty();
    }
}