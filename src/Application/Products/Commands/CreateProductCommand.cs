using AutoMapper;
using MediatR;
using netca.Application.Common.Interfaces;
using netca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netca.Application.Products.Commands;

/// <summary>
/// 
/// </summary>
public class CreateProductCommand : IRequest<Unit>
{
    public string Name { get; set; }
    public int Price { get; set; }
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public CreateProductHandler(IApplicationDbContext context, IMapper mapper)
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
        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Price = request.Price,
            };

            _context.Products.Add(newProduct);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
