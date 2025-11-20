using ezyGo.Auth.Storage.Entities;
using ezyGo.Auth.Storage.Sql;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Auth.Storage.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _db;
    
    public AuthRepository(AuthDbContext dbContext)
    {
        _db = dbContext;
    } 
    
    public async Task AddAsync(UserEntity user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid id)
    {
        return await _db.Users
            .Where(u => u.Id == id && u.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await _db.Users
            .Where(u => u.Email == email && u.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByUsernameAsync(string username)
    {
        return await _db.Users
            .Where(u => u.UserName == username && u.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UserExistsByEmailAsync(string email)
    {
        return await _db.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> UserExistsByUsernameAsync(string username)
    {
        return await _db.Users
            .AnyAsync(u => u.UserName == username);
    }

    public async Task UpdateUserAsync(UserEntity user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user != null)
        {
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }
}
