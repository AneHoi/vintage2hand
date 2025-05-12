using domain;

public interface IListingService
{
    Task<List<Listing>> GetListingsByCategoryAsync(string category);
    Task<Listing> GetListingByIdAsync(Guid id);
    Task CreateListingAsync(Listing listing);
    Task UpdateListingAsync(Listing listing);
    Task DeleteListingAsync(Guid id);
}