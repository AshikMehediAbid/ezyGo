using ezyGo.Auth.Domain.Models;

namespace ezyGo.Auth.Domain.Managers;

public interface IAuthService
{
    Task UserRegisterAsync(UserRegister newUser);
    Task<string> UserLoginAsync(UserLogin userLogin);
    Task<string> UserLogoutAsync();

}
