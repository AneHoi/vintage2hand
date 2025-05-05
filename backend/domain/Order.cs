namespace domain;

public class Order
{
    public int PurchaseId { get; set; }
    public Guid ListingId { get; set; }
    public Guid BuyierId { get; set; }
    public double Price { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}