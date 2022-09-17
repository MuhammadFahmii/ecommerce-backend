// ------------------------------------------------------------------------------------
// CreateTodoItemCommandValidator.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using FluentValidation;

namespace netca.Application.TodoItems.Commands.CreateTodoItem;

/// <summary>
/// CreateTodoItemCommandValidator
/// </summary>
public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoItemCommandValidator"/> class.
    /// </summary>
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().NotEmpty();
        RuleFor(x => x.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}