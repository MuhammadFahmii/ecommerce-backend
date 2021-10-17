// ------------------------------------------------------------------------------------
// UpdateTodoItemCommandValidator.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using FluentValidation;

namespace netca.Application.TodoItems.Commands.UpdateTodoItem
{
    /// <summary>
    /// UpdateTodoItemCommandValidator
    /// </summary>
    public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
    {
        /// <summary>
        /// UpdateTodoItemCommandValidator
        /// </summary>
        public UpdateTodoItemCommandValidator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
