namespace domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public Guid BuyerId { get; set; }
    public double Price { get; set; }
    public OrderStatus OrderStatus { get; set; }
}