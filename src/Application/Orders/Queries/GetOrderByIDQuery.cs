using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using netca.Application.Common.Behaviors;
using netca.Application.Common.Extensions;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Vms;
using netca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netca.Application.Orders.Queries;

/// <summary>
/// 
/// </summary>
[RetryPolicy(RetryCount = 2, SleepDuration = 500)]
public class GetOrderByIDQuery : IRequest<DocumentRootJson<OrdersVm>>
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIDQuery, DocumentRootJson<OrdersVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        public GetOrderByIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DocumentRootJson<OrdersVm>> Handle(GetOrderByIDQuery request, CancellationToken cancellationToken)
        {
            var ordersEntity = await _context.Orders            
                .Where(o => o.Id.Equals(request.Id))
                .Include(o => o.Voucher)
                .FirstOrDefaultAsync(cancellationToken);

            OrdersVm ordersVm = _mapper.Map<OrdersVm>(ordersEntity);

            if(ordersEntity?.Voucher?.Name == "Kurang 10k")
            {
                ordersVm.AfterDisc = ordersEntity.TotalPaid - 10000;
            }

            var productVm = await _context.OrderProducts
                .Where(op => op.OrderID.Equals(request.Id))
                .ProjectTo<ProductVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            ordersVm.Products = productVm;

            return JsonApiExtensions.ToJsonApi<OrdersVm>(ordersVm);
        }
    }
}
