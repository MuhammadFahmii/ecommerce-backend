using AutoMapper;
using ecommerce.Application.Common.Mappings;
using ecommerce.Domain.Entities;

namespace ecommerce.Application.Dtos;

/// <summary>
/// Product order dto
/// </summary>
public class ProductOrderDto :IMapFrom<OrderProduct>
{
    /// <summary>
    /// 
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="profile"></param>
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductOrderDto, OrderProduct>();
    }
}