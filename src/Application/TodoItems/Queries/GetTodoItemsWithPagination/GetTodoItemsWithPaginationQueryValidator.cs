// ------------------------------------------------------------------------------------
// GetTodoItemsWithPaginationQueryValidator.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using FluentValidation;

namespace netca.Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    /// <summary>
    /// GetTodoItemsWithPaginationQueryValidator
    /// </summary>
    public class GetTodoItemsWithPaginationQueryValidator : AbstractValidator<GetTodoItemsWithPaginationQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTodoItemsWithPaginationQueryValidator"/> class.
        /// </summary>
        public GetTodoItemsWithPaginationQueryValidator()
        {
            RuleFor(x => x.ListId)
                .NotEmpty().WithMessage("ListId is required.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}
