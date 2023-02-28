﻿namespace ecommerce.Domain.Entities;

public record OrderProduct
{
    public Guid OrderID { get; set; }
    public Order Order { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}