using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APBDProjekt.DTOs.Security;
using APBDProjekt.Entities;
using APBDProjekt.Entities.Configs;
using APBDProjekt.Helpers;
using APBDProjekt.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace APBDProjekt.Services;

public class SecurityService: ISecurityService
{
    private readonly ISecurityRepository _securityRepository;

    public SecurityService(ISecurityRepository securityRepository)
    {
        _securityRepository = securityRepository;
    }

    public async Task<bool> UserExistsAsync(string login, CancellationToken token)
    {
        return await _securityRepository.UserExistsAsync(login, token);
    }

    public async Task<Role?> GetRoleAsync(string roleName, CancellationToken token)
    {
        return await _securityRepository.GetRoleAsync(roleName, token);
    }

    public bool IsRoleInDb(Role? role)
    {
        return _securityRepository.IsRoleInDb(role);
    }

    public AppUser? CreateNewAppUser(RegisterRequest request, Tuple<string, string> hash, int roleId)
    {
        return _securityRepository.CreateNewAppUser(request, hash, roleId);
    }

    public async Task<int> AddNewUserAsync(AppUser? user, CancellationToken token)
    {
        return await _securityRepository.AddNewUserAsync(user, token);
    }

    public async Task<string> RegisterUserAsync(RegisterRequest request, CancellationToken token)
    {
        if (await UserExistsAsync(request.Login, token))
        {
            throw new Exception($"Error: User {request.Login} is already registered.");
        }

        Role? role = await GetRoleAsync(request.Role, token);

        if (!IsRoleInDb(role))
        {
            throw new Exception($"Error: Cannot find role with name {request.Role}");
        }
        
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);

        AppUser? user = CreateNewAppUser(request, hashedPasswordAndSalt, role.RoleId);

        await AddNewUserAsync(user, token);
        return "New user successfully registered.";
    }
    

    public async Task<AppUser?> GetUserAsync(string login, CancellationToken token)
    {
        return await _securityRepository.GetUserAsync(login, token);
    }

    public SymmetricSecurityKey NewSymmetricSecurityKey()
    {
        return _securityRepository.NewSymmetricSecurityKey();
    }

    public SigningCredentials NewSigningCredentials(SymmetricSecurityKey key)
    {
        return _securityRepository.NewSigningCredentials(key);
    }

    public JwtSecurityToken NewJwtSecurityToken(Claim[] claims, SigningCredentials credentials)
    {
        return _securityRepository.NewJwtSecurityToken(claims, credentials);
    }

    public async Task<int> RefreshTokensAsync(AppUser user, CancellationToken token)
    {
        return await _securityRepository.RefreshTokensAsync(user, token);
    }

    public string Response(JwtSecurityToken token, AppUser user)
    {
        return _securityRepository.Response(token, user);
    }

    public async Task<string> LoginUserAsync(LoginRequest request, CancellationToken token)
    {
        AppUser? user = await GetUserAsync(request.Login, token);
        
        string? passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(request.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            throw new UnauthorizedAccessException("Error: Unauthorized access.");
        }

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Login),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        SymmetricSecurityKey key = NewSymmetricSecurityKey();
        SigningCredentials credentials = NewSigningCredentials(key);

        JwtSecurityToken jwtSecurityToken = NewJwtSecurityToken(claims, credentials);

        await RefreshTokensAsync(user, token);

        return Response(jwtSecurityToken, user);
    }

    public async Task<AppUser?> GetUserAsyncByToken(string refreshToken, CancellationToken token)
    {
        return await _securityRepository.GetUserAsyncByToken(refreshToken, token);
    }

    public bool IsRefreshTokenExpired(DateTime? date)
    {
        return _securityRepository.IsRefreshTokenExpired(date);
    }

    public async Task<string> RefreshAsync(RefreshTokenRequest refreshToken, CancellationToken token)
    {
        AppUser? user = await GetUserAsyncByToken(refreshToken.RefreshToken, token);
        Console.WriteLine(user);
        
        if (user == null)
        {
            throw new SecurityTokenException("Error: Invalid refresh token");
        }

        if (IsRefreshTokenExpired(user.RefreshTokenExp))
        {
            throw new SecurityTokenException("Error: Refresh token expired");
        }
        
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };
        
        SymmetricSecurityKey key = NewSymmetricSecurityKey();
        SigningCredentials credentials = NewSigningCredentials(key);

        JwtSecurityToken jwtSecurityToken = NewJwtSecurityToken(claims, credentials);
        
        await RefreshTokensAsync(user, token);

        return Response(jwtSecurityToken, user);
    }
}