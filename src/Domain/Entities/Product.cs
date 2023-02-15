namespace netca.Domain.Entities;

public record Product : BaseAuditableEntity
{
    public String? Name { get; set; }
    public int? Price { get; set; }
    public enum Status
    {
        OutOfStock,
        InStock,
        RunningLow
    }
    public IList<OrderProduct> OrderProducts { get; set; }
}