namespace application.Queries;

public record ListingSummaryDto(
    Guid Id,
    string SellerName, // Note how the seller's name has been denormalized
    string Title,
    string Description,
    string Address,
    double Price,
    string ListingStatus,
    string OrderStatus,
    List<string> Images
    );