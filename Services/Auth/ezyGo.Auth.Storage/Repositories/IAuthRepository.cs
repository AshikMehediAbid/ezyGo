using ezyGo.Auth.Storage.Entities;

namespace ezyGo.Auth.Storage.Repositories;

public interface IAuthRepository
{
    Task AddAsync(UserEntity user);
    Task<UserEntity?> GetUserByIdAsync(Guid id);
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<UserEntity?> GetUserByUsernameAsync(string username);
    Task<bool> UserExistsByEmailAsync(string email);
    Task<bool> UserExistsByUsernameAsync(string username);
    Task UpdateUserAsync(UserEntity user);
    Task DeleteUserAsync(Guid id);
}
 