namespace netca.Domain.Entities;
public record Voucher : BaseAuditableEntity
{
    public string? Name { get; set; }
}