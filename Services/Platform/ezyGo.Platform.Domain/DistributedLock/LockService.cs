using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ezyGo.Platform.Domain.DistributedLock;

public class LockService : ILockService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;
    private readonly ILogger<LockService> _logger;
    public LockService(IConnectionMultiplexer connectionMultiplexer, ILogger<LockService> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<bool> ReleaseSeatAsync(string key)
    {
        try
        {
            bool isDeleted = await _database.KeyDeleteAsync(key);
            return isDeleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing seat with key {Key}", key);
            throw;
        }
    }

    public async Task<bool> ReserveSeatAsync(string key, string value, TimeSpan expiry)
    {
        try
        {
            bool isSet = await _database.StringSetAsync(key, value, expiry, When.NotExists);
            return isSet;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing seat with key {Key}", key);
            throw;
        }
    }
}
