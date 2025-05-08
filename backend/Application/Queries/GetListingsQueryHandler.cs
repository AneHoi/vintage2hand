using domain.Interfaces;

namespace application.Queries;

public class GetListingsQueryHandler
{
    private readonly IListingReadRepository _listingReadRepository;
    private readonly ICacheService _cacheService;
    
    public GetListingsQueryHandler(IListingReadRepository listingReadRepository, ICacheService cacheService)
    {
        _listingReadRepository = listingReadRepository;
        _cacheService = cacheService;
    }
    
    public async Task<IEnumerable<ListingSummaryDto>> Handle(GetListingsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"listings:browse:cat{request.CategoryFilter}:term{request.SearchTerm}"; // syntax = dev convention

        var cachedResult = await _cacheService.GetAsync<IEnumerable<ListingSummaryDto>>(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult;
        }
        
        var listings = await _listingReadRepository.GetActiveListingsAsync(
            request.CategoryFilter,
            request.SearchTerm,
            cancellationToken
        );

        await _cacheService.SetAsync(cacheKey, listings, TimeSpan.FromMinutes(10));

        return listings;
    }
}