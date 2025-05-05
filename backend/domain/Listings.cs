namespace domain;

public class Listings
{
    public Guid Id { get; set; }
    public Guid userId { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public double Price { get; set; }

    public Status Status { get; set; } 
    public List<ListingImage> Image { get; set; } = [];
}


//key value attributes