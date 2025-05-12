using domain;

namespace domain.Interfaces;
public interface IListingRepository
{
    Task<List<Listing>> GetListingsByCategoryAsync(string category);
    Task<Listing> GetListingByIdAsync(Guid listingId);
    Task CreateListingAsync(Listing listing);
    Task UpdateListingAsync(Listing listing);
    Task DeleteListingAsync(Guid listingId);
}