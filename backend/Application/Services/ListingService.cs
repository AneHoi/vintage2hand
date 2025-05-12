using domain;
using application.Interfaces;
using domain.Interfaces;
using Microsoft.Extensions.Logging;


namespace application.Services;

public class ListingService : IListingService
{
    private readonly IListingRepository _listingRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<ListingService> _logger;

    public ListingService(IListingRepository listingRepository, ICacheService cacheService, ILogger<ListingService> logger)
    {
        _listingRepository = listingRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<List<Listing>> GetListingsByCategoryAsync(string category)
    {
        var cacheKey = $"listing:{category}";
        await _cacheService.GetOrSetAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Cache miss for category {Category}", category);
            return await _listingRepository.GetListingsByCategoryAsync(category);
        });
    }

    public async Task<Listing> GetListingByIdAsync(Guid id)
    {
        var cacheKey = $"listing:{id}";
        return await _cacheService.GetOrSetAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Cache miss for listing {Id}", id);
            return await _listingRepository.GetListingByIdAsync(id);
        });
    }

    public async Task CreateListingAsync(Listing listing)
    {
        await _listingRepository.CreateListingAsync(listing);
        await _cacheService.RemoveAsync($"listing:{listing.ListingStatus}");
        _logger.LogInformation("Created listing {Id}", listing.Id);
    }

    public async Task UpdateListingAsync(Listing listing)
    {
        await _listingRepository.UpdateListingAsync(listing);
        await _cacheService.RemoveAsync($"listing:{listing.Id}");
        await _cacheService.RemoveAsync($"listing:{listing.ListingStatus}");
        _logger.LogInformation("Updated listing {Id}", listing.Id);
    }

    public async Task DeleteListingAsync(Guid id)
    {
        await _listingRepository.DeleteListingAsync(id);
        await _cacheService.RemoveAsync($"listing:{id}");
        _logger.LogInformation("Deleted listing {Id}", id);
    }
}
