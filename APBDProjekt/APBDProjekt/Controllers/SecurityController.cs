using APBDProjekt.DTOs.Security;
using APBDProjekt.Entities;
using APBDProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBDProjekt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecurityController: ControllerBase
{
    private readonly ISecurityService _securityService;

    public SecurityController(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAppUserAsync(RegisterRequest model, CancellationToken token)
    {
        return Ok(await _securityService.RegisterUserAsync(model, token));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAppUserAsync(LoginRequest loginRequest, CancellationToken token)
    {
        return Ok(await _securityService.LoginUserAsync(loginRequest, token));
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest refreshToken, CancellationToken token)
    {
        return Ok(await _securityService.RefreshAsync(refreshToken, token));
    }
}