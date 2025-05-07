using System.ComponentModel.DataAnnotations;

namespace domain;

public class Listings
{
    public Guid Id { get; set; }
    public Guid userId { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    
    [Range(0, 900000, ErrorMessage = "The price cannot be negative.")]
    public double Price { get; set; }
    
    public Status Status { get; set; } 
    public List<ListingImage> Image { get; set; } = [];
}


//key value attributes