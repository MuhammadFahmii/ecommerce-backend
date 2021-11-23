// ------------------------------------------------------------------------------------
// GetTodoItemsWithPaginationQuery.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Mappings;
using netca.Application.Common.Models;

namespace netca.Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    /// <summary>
    /// GetTodoItemsWithPaginationQuery
    /// </summary>
    public class GetTodoItemsWithPaginationQuery : IRequest<DocumentRootJson<List<TodoItemBriefVm>>>
    {
        /// <summary>
        /// Gets or sets listId
        /// </summary>
        [BindRequired]
        public Guid ListId { get; set; }

        /// <summary>
        /// Gets or sets pageNumber
        /// </summary>
        public int PageNumber { get; set; } = Constants.DefaultPageNumber;

        /// <summary>
        /// Gets or sets pageSize
        /// </summary>
        public int PageSize { get; set; } = Constants.DefaultPageSize;
    }

    /// <summary>
    /// GetTodoItemsWithPaginationQueryHandler
    /// </summary>
    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetTodoItemsWithPaginationQuery, DocumentRootJson<List<TodoItemBriefVm>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTodoItemsWithPaginationQueryHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DocumentRootJson<List<TodoItemBriefVm>>> Handle(GetTodoItemsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.TodoItems!
                .Where(x => x.ListId == request.ListId)
                .OrderBy(x => x.Title)
                .ProjectTo<TodoItemBriefVm>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
