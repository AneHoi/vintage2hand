using System.ComponentModel.DataAnnotations;

namespace domain;

public class SellerReview
{
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
    public int Rating { get; set; }
    
    public Guid SellerId { get; set; }
    public Guid ReviewerId { get; set; }
}