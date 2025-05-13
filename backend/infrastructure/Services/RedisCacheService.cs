using System.Text.Json;
using application.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
    {
        _database = redis.GetDatabase();
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (!value.HasValue)
        {
            _logger.LogInformation("Cache miss for key {Key}", key);
            return default;
        }

        _logger.LogInformation("Cache hit for key {Key}", key);
        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
    {
        var json = JsonSerializer.Serialize(value);
        var expiry = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(10);
        await _database.StringSetAsync(key, json, expiry);
        _logger.LogInformation("Cache set for key {Key} with expiration {Expiry}", key, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
        _logger.LogInformation("Cache removed for key {Key}", key);
    }
}