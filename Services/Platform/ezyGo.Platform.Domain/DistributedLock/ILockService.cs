namespace ezyGo.Platform.Domain.DistributedLock;

public interface ILockService
{
    Task<bool> ReserveSeatAsync(string key, string value, TimeSpan expiry);
    Task<bool> ReleaseSeatAsync(string key);
}
