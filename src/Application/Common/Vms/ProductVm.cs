using AutoMapper;
using netca.Application.Common.Mappings;
using netca.Application.Orders.Queries;
using netca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netca.Application.Common.Vms;

/// <summary>
/// Product Vm
/// </summary>
public record ProductVm : IMapFrom<Product>
{
    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="profile"></param>
    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderProduct, ProductVm>()
            .ForMember(m=>m.Name, conf => conf.MapFrom(c=>c.Product.Name));
    }
}
