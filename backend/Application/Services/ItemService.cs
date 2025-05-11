public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly RedisCacheService _cacheService;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IItemRepository itemRepository, RedisCacheService cacheService, ILogger<ItemService> logger)
    {
        _itemRepository = itemRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<List<Item>> GetItemsByCategoryAsync(string category)
    {
        var cacheKey = $"item-listings:{category}";
        
        var cachedItems = await _cacheService.GetOrSetAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Cache miss for items by category {Category}, fetching from DB", category);
            return await _itemRepository.GetItemsByCategoryAsync(category);
        });

        return cachedItems;
    }

    public async Task<Item> GetItemByIdAsync(Guid itemId)
    {
        var cacheKey = $"item:{itemId}";
        var cachedItem = await _cacheService.GetOrSetAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Cache miss for item with ID {ItemId}, fetching from DB", itemId);
            return await _itemRepository.GetItemByIdAsync(itemId);
        });

        return cachedItem;
    }

    public async Task CreateItemAsync(Item item)
    {
        try
        {
            await _itemRepository.CreateItemAsync(item);
            var cacheKey = $"item-listings:{item.Category}";
            await _cacheService.RemoveAsync(cacheKey);

            _logger.LogInformation("Item with ID {ItemId} created and cache invalidated", item.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create item with ID {ItemId}", item.Id);
            throw;
        }
    }

    public async Task UpdateItemAsync(Item item)
    {
        try
        {
            await _itemRepository.UpdateItemAsync(item);
            var itemCacheKey = $"item:{item.Id}";
            await _cacheService.RemoveAsync(itemCacheKey);
            var categoryCacheKey = $"item-listings:{item.Category}";
            await _cacheService.RemoveAsync(categoryCacheKey);

            _logger.LogInformation("Item with ID {ItemId} updated and cache invalidated", item.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update item with ID {ItemId}", item.Id);
            throw;
        }
    }

    public async Task DeleteItemAsync(Guid itemId)
    {
        try
        {
            await _itemRepository.DeleteItemAsync(itemId);
            var itemCacheKey = $"item:{itemId}";
            await _cacheService.RemoveAsync(itemCacheKey);

            _logger.LogInformation("Item with ID {ItemId} deleted and cache invalidated", itemId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete item with ID {ItemId}", itemId);
            throw;
        }
    }
}
