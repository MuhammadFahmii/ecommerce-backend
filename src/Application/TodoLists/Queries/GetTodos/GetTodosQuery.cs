// ------------------------------------------------------------------------------------
// GetTodosQuery.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.EntityFrameworkCore;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Application.Common.Vms;
using netca.Domain.Enums;

namespace netca.Application.TodoLists.Queries.GetTodos
{
    /// <summary>
    /// GetTodosQuery
    /// </summary>
    public class GetTodosQuery : QueryModel, IRequest<DocumentRootJson<TodosVm>>
    {
    }

    /// <summary>
    /// GetTodosQueryHandler
    /// </summary>
    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, DocumentRootJson<TodosVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTodosQueryHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
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
        public async Task<DocumentRootJson<TodosVm>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var countEntity = await _context.TodoLists!
                .AsNoTracking()
                .QueryWithoutLimit(request)
                .Select(x => x.Id)
                .CountAsync(cancellationToken);
            var vm = new TodosVm
            {
                PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                     .Cast<PriorityLevel>()
                     .Select(p => new PriorityLevelVm { Value = (int)p, Name = p.ToString() })
                     .ToList(),

                Lists = await _context.TodoLists!
                     .AsNoTracking()
                     .Query(request)
                     .ProjectTo<TodoListVm>(_mapper.ConfigurationProvider)
                     .OrderBy(t => t.Title).ToListAsync(cancellationToken)
            };

            return JsonApiExtensions.ToJsonApiPaginated(vm, countEntity, request?.PageNumber ?? Constants.DefaultPageNumber, request?.PageSize ?? Constants.DefaultPageSize);
        }
    }
}
