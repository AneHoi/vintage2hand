public interface IItemService
{
    Task<List<Item>> GetItemsByCategoryAsync(string category);
    Task<Item> GetItemByIdAsync(Guid itemId);
    Task CreateItemAsync(Item item);
    Task UpdateItemAsync(Item item);
    Task DeleteItemAsync(Guid itemId);
}