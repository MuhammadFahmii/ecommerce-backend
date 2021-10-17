// ------------------------------------------------------------------------------------
// GetTodoItemsWithPaginationQuery.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JsonApiSerializer.JsonApi;
using MediatR;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Mappings;

namespace netca.Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    /// <summary>
    /// GetTodoItemsWithPaginationQuery
    /// </summary>
    public class GetTodoItemsWithPaginationQuery : IRequest<DocumentRootJson<List<TodoItemBriefVm>>>
    {
        /// <summary>
        /// ListId
        /// </summary>
        [Required]
        public Guid ListId { get; set; }
        
        /// <summary>
        /// PageNumber
        /// </summary>
        [Required]
        public int PageNumber { get; set; } = 1;
        
        /// <summary>
        /// PageSize
        /// </summary>
        [Required]
        public int PageSize { get; set; } = 10;
    }
    
    /// <summary>
    /// GetTodoItemsWithPaginationQueryHandler
    /// </summary>
    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetTodoItemsWithPaginationQuery, DocumentRootJson<List<TodoItemBriefVm>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        /// <summary>
        /// GetTodoItemsWithPaginationQueryHandler
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
            return await _context.TodoItems
                .Where(x => x.ListId == request.ListId)
                .OrderBy(x => x.Title)
                .ProjectTo<TodoItemBriefVm>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(  -1,request.PageNumber, request.PageSize);
        }
    }
}
