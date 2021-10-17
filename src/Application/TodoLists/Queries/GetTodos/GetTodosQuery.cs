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
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Vms;
using netca.Domain.Enums;

namespace netca.Application.TodoLists.Queries.GetTodos
{
    /// <summary>
    /// GetTodosQuery
    /// </summary>
    public class GetTodosQuery : IRequest<DocumentRootJson<TodosVm>>
    {
    }
    
    /// <summary>
    /// GetTodosQueryHandler
    /// </summary>
    public class  GetTodosQueryHandler: IRequestHandler<GetTodosQuery, DocumentRootJson<TodosVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private  readonly ILogger<GetTodosQueryHandler> _logger;
        
        /// <summary>
        /// GetTodosQueryHandler
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetTodosQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DocumentRootJson<TodosVm>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            _logger.LogWarning("masuk2");
           var vm =  new TodosVm
            {
                PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                    .Cast<PriorityLevel>()
                    .Select(p => new PriorityLevelVm { Value = (int)p, Name = p.ToString() })
                    .ToList(),

                Lists = await _context.TodoLists
                    .AsNoTracking()
                    .ProjectTo<TodoListVm>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.Title)
                    .ToListAsync(cancellationToken)
            };
           return JsonApiExtensions.ToJsonApi(vm);
        }
    }
}
