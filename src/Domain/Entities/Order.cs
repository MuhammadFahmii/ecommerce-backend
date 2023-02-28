namespace ecommerce.Domain.Entities;

public record Order : BaseAuditableEntity
{
    public string? Status { get; set; }
    public int TotalPaid { get; set; }
    public IList<OrderProduct> OrderProducts { get; set; }
    public Voucher? Voucher { get; set; }
}