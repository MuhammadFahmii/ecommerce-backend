using MediatR;
using ecommerce.Application.Common.Interfaces;
using ecommerce.Domain.Events;
using System.ComponentModel.DataAnnotations;
using ecommerce.Domain.Entities;
using ecommerce.Application.Dtos;
using AutoMapper;

namespace ecommerce.Application.Orders.Commands;

/// <summary>
/// Create Order Command
/// </summary>
public class CreateOrderCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets or sets status
    /// </summary>
    [Required]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets total paid
    /// </summary>
    [Required]
    public int TotalPaid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<ProductOrderDto> ProductOrderDto { get; set; }

    /// <summary>
    ///
    /// </summary>
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrderCommandHandler"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userAuthorizationService"></param>
        public CreateOrderCommandHandler(IApplicationDbContext context, IUserAuthorizationService userAuthorizationService, IMapper mapper)
        {
            _context = context;
            _userAuthorizationService = userAuthorizationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Handle create order command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = new Order
            {
                Status= request.Status,
                TotalPaid = request.TotalPaid,
                CreatedBy = _userAuthorizationService.GetAuthorizedUser().UserId
            };

            _context.Orders.Add(newOrder);
            
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var item in request.ProductOrderDto)
            {
                var newProductOrder = new OrderProduct { ProductId = item.ProductId, OrderID = newOrder.Id, Quantity=item.Quantity };

                _context.OrderProducts.Add(newProductOrder);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}