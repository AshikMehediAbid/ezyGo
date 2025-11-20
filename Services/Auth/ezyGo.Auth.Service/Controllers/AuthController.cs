using ezyGo.Auth.Domain.Exceptions;
using ezyGo.Auth.Domain.Managers;
using ezyGo.Auth.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using ValidationException = ezyGo.Auth.Domain.Exceptions.ValidationException;

namespace ezyGo.Auth.Service.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost]
    [Route("sign-up")]
    public async Task<IActionResult> Register(UserRegister newUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) 
                });
            }

            await _authService.UserRegisterAsync(newUser);
            
            return Ok(new { 
                message = "User registered successfully",
                user = new { 
                    newUser.UserName, 
                    newUser.Email, 
                    newUser.Role 
                }
            });
        }
        catch (UserAlreadyExistsException ex)
        {
            _logger.LogWarning("Registration failed - user already exists: {Email}", newUser.Email);
            return Conflict(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Registration failed - validation error: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration");
            return StatusCode(500, new { message = "An internal server error occurred" });
        }
    }

    [HttpPost]
    [Route("sign-in")]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) 
                });
            }

            var token = await _authService.UserLoginAsync(userLogin);
            
            return Ok(new { 
                message = "Login successful",
                token = token,
                tokenType = "Bearer"
            });
        }
        catch (InvalidCredentialsException ex)
        {
            _logger.LogWarning("Login failed - invalid credentials for user: {Email}", userLogin.Email);
            return Unauthorized(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Login failed - validation error: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login");
            return StatusCode(500, new { message = "An internal server error occurred" });
        }
    }

}
