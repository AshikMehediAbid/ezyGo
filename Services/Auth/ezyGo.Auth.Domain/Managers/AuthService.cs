using AutoMapper;
using ezyGo.Auth.Domain.Exceptions;
using ezyGo.Auth.Domain.Models;
using ezyGo.Auth.Storage.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ValidationException = ezyGo.Auth.Domain.Exceptions.ValidationException;

namespace ezyGo.Auth.Domain.Managers;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository authRepository, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> UserLoginAsync(UserLogin userLogin)
    {
        try
        {
            _logger.LogInformation("Attempting login for user: {Email}", userLogin.Email);

            // Validate input
            var validationContext = new ValidationContext(userLogin);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(userLogin, validationContext, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }

            var user = await _authRepository.GetUserByEmailAsync(userLogin.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed - user not found: {Email}", userLogin.Email);
                throw new InvalidCredentialsException();
            }

            var passwordHasher = new PasswordHasher();
            var result = passwordHasher.VerifyHashedPassword(user.PasswordHash, userLogin.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login attempt failed - invalid password for user: {Email}", userLogin.Email);
                throw new InvalidCredentialsException();
            }

            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Email, userLogin.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            _logger.LogInformation("Login successful for user: {Email}", userLogin.Email);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (AuthException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for user: {Email}", userLogin.Email);
            throw new AuthException("An error occurred during login", ex);
        }
    }

    public Task<string> UserLogoutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task UserRegisterAsync(UserRegister newUser)
    {
        try
        {
            _logger.LogInformation("Attempting registration for user: {Email}", newUser.Email);

            // Validate input
            var validationContext = new ValidationContext(newUser);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newUser, validationContext, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }

            // Check if user already exists
            var isExist = await _authRepository.UserExistsByEmailAsync(newUser.Email);
            if (isExist)
            {
                _logger.LogWarning("Registration failed - user already exists: {Email}", newUser.Email);
                throw new UserAlreadyExistsException(newUser.Email);
            }

            // Check if username already exists
            var usernameExists = await _authRepository.UserExistsByUsernameAsync(newUser.UserName);
            if (usernameExists)
            {
                _logger.LogWarning("Registration failed - username already exists: {Username}", newUser.UserName);
                throw new ValidationException("Username already exists");
            }

            var passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.HashPassword(newUser.Password);

            var entityUser = _mapper.Map<Storage.Entities.UserEntity>(newUser);
            entityUser.PasswordHash = hashedPassword;
            entityUser.Id = Guid.NewGuid();
            entityUser.CreatedAt = DateTime.UtcNow;
            entityUser.IsActive = true;

            await _authRepository.AddAsync(entityUser);
            _logger.LogInformation("Registration successful for user: {Email}", newUser.Email);
        }
        catch (AuthException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration for user: {Email}", newUser.Email);
            throw new AuthException("An error occurred during registration", ex);
        }
    }
}
