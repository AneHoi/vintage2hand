namespace domain;

public record ListingCreatedEvent()
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public double Price { get; set; }
    public ListingStatus ListingStatus { get; set; } 
    public List<ListingImage> Images { get; set; }
    
    
}