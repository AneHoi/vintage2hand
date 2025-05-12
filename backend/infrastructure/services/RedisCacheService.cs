using System.Text.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using application.Interfaces;
using System; 
using System.Threading.Tasks;

namespace infrastructure.services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly TimeSpan _defaultExpiry = TimeSpan.FromMinutes(10);

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _database = redis.GetDatabase();
            _logger = logger;
        }
        
        public async Task<T> GetAsync<T>(string key)
        {
            var cached = await _database.StringGetAsync(key);
            if (cached.HasValue)
            {
                try
                {
                    
                    var value = JsonSerializer.Deserialize<T>(cached);
                    _logger.LogDebug("Cache hit for key {Key}", key);
                    return value ?? default(T);
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to deserialize cache data for key {Key}. Returning default.", key);
                    return default(T);
                }
            }

            _logger.LogDebug("Cache miss for key {Key}", key);
            return default(T);
        }
        
        public async Task SetAsync<T>(string key, T value)
        {
            if (value == null)
            {
                _logger.LogWarning("Attempted to set null value for key {Key}. Removing key instead.", key);
                await RemoveAsync(key);
                return;
            }

            try
            {
                var json = JsonSerializer.Serialize(value);
                var success = await _database.StringSetAsync(key, json, _defaultExpiry);
                if (success)
                {
                    _logger.LogDebug("Cache set for key {Key} with default expiry {Expiry}", key, _defaultExpiry);
                }
                else
                {
                     _logger.LogWarning("Failed to set cache for key {Key}", key);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to serialize value for key {Key}. Cache not set.", key);
            }
        }
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> fetchData)
        {
            var cached = await _database.StringGetAsync(key);
            if (cached.HasValue)
            {
                 try
                {
                    var value = JsonSerializer.Deserialize<T>(cached);
                    _logger.LogDebug("Cache hit for key {Key} in GetOrSetAsync", key);
                    return value ?? default(T);
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to deserialize cache data for key {Key} in GetOrSetAsync. Fetching data instead.", key);
                }
            }

            _logger.LogInformation("Cache miss for key {Key}. Fetching data.", key);
            var result = await fetchData(); 
            
            if (result != null)
            {
                 try
                {
                    var json = JsonSerializer.Serialize(result);
                    var success = await _database.StringSetAsync(key, json, _defaultExpiry);
                     if (success)
                    {
                         _logger.LogDebug("Cache set for key {Key} after fetch with default expiry {Expiry}", key, _defaultExpiry);
                    }
                     else
                    {
                         _logger.LogWarning("Failed to set cache for key {Key} after fetch.", key);
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to serialize fetched data for key {Key}. Cache not set.", key);
                }
            }
            else
            {
                 _logger.LogWarning("Fetched data for key {Key} was null. Not caching.", key);
            }
            
            return result ?? default(T);
        }
        public async Task RemoveAsync(string key)
        {
            var deleted = await _database.KeyDeleteAsync(key);
            if (deleted)
            {
                _logger.LogDebug("Removed cache key {Key}", key);
            }
            else
            {
                _logger.LogDebug("Attempted to remove cache key {Key}, but it did not exist.", key);
            }
        }
        
        public async Task RemoveByPatternAsync(string pattern)
        {
            
             try
             {
                 var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
                 var keys = server.Keys(_database.Database, pattern: pattern).ToArray();

                 if (keys.Length > 0)
                 {
                     await _database.KeyDeleteAsync(keys);
                     _logger.LogInformation("Removed {Count} cache keys matching pattern {Pattern}", keys.Length, pattern);
                 }
                 else
                 {
                      _logger.LogInformation("No cache keys found matching pattern {Pattern}", pattern);
                 }
             }
             catch(Exception ex)
             {
                 _logger.LogError(ex, "Error removing keys by pattern {Pattern}", pattern);
             }
        }
    }
}