// ------------------------------------------------------------------------------------
// ExportTodosQuery.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ecommerce.Application.Common.Behaviors;
using ecommerce.Application.Common.Interfaces;

namespace ecommerce.Application.TodoLists.Queries.ExportTodos
{
    /// <summary>
    /// ExportTodosQuery
    /// </summary>
    [RetryPolicy(RetryCount = 2, SleepDuration = 500)]
    public class ExportTodosQuery : IRequest<ExportTodosVm>
    {
        /// <summary>
        /// Gets or sets listId
        /// </summary>
        public Guid ListId { get; set; }
    }

    /// <summary>
    /// ExportTodosQueryHandler
    /// </summary>
    public class ExportTodosQueryHandler : IRequestHandler<ExportTodosQuery, ExportTodosVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICsvFileBuilder _fileBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportTodosQueryHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="fileBuilder"></param>
        public ExportTodosQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder)
        {
            _context = context;
            _mapper = mapper;
            _fileBuilder = fileBuilder;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ExportTodosVm> Handle(ExportTodosQuery request, CancellationToken cancellationToken)
        {
            var vm = new ExportTodosVm();

            var records = await _context.TodoItems
                .Where(t => t.ListId == request.ListId)
                .ProjectTo<TodoItemRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            vm.Content = _fileBuilder.BuildTodoItemsFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = "TodoItems.csv";

            return await Task.FromResult(vm);
        }
    }
}
