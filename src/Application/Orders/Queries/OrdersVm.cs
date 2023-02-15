using AutoMapper;
using netca.Application.Common.Mappings;
using netca.Application.Common.Vms;
using netca.Domain.Entities;

namespace netca.Application.Orders.Queries;

/// <summary>
/// Orders Vm
/// </summary>
public record OrdersVm : IMapFrom<Order>
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Products
    /// </summary>
    public IList<ProductVm>? Products { get; set; }

    public int TotalPaid { get; set; }

    /// <summary>
    /// After disc
    /// </summary>
    public int? AfterDisc { get; set; }

    /// <summary>
    /// Mapping
    /// </summary>
    /// <param name="profile"></param>
    public void Mapping (Profile profile)
    {
        profile.CreateMap<Order, OrdersVm>();
    }
}