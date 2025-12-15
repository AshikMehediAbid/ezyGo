using ezyGo.Platform.Domain.DistributedLock;
using ezyGo.Platform.Domain.Managers.Interfaces;
using Microsoft.Extensions.Logging;

namespace ezyGo.Platform.Domain.Managers;

public class SeatService : ISeatService
{
    private readonly ILockService _lockService;
    private readonly ILogger<SeatService> _logger;
    public SeatService(ILockService lockSerice,ILogger<SeatService> logger)
    {
        _lockService = lockSerice;
        _logger = logger;
    }
    public async Task<bool> ReleaseSeatAsync(int seatId)
    {
        var lockKey = $"seat_lock_{seatId}";
        return await _lockService.ReleaseSeatAsync(lockKey);
    }

    public async Task<bool> ReserveSeatAsync(int seatId)
    {
        string lockValue = Guid.NewGuid().ToString();
        TimeSpan expiry = TimeSpan.FromMinutes(5);
        var lockKey = $"seat_lock_{seatId}";
      

        bool isAvailable = await _lockService.ReserveSeatAsync(lockKey, lockValue, expiry);

        if(!isAvailable)
        {
            _logger.LogWarning("Seat {SeatId} is already reserved.", seatId);
            return false;
        }

        return true;
    }
}
