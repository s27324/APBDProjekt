using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBDProjekt.DTOs.Security;
using APBDProjekt.Entities;
using APBDProjekt.Entities.Configs;
using APBDProjekt.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBDProjekt.Repositories;

public class SecurityRepository: ISecurityRepository
{
    private readonly SoftwareDistributionDbContext _distributionDbContext;

    public SecurityRepository(SoftwareDistributionDbContext distributionDbContext)
    {
        _distributionDbContext = distributionDbContext;
    }

    public async Task<bool> UserExistsAsync(string login, CancellationToken token)
    {
        return await _distributionDbContext.AppUsers.AnyAsync(u => u.Login == login, token);
    }

    public async Task<Role?> GetRoleAsync(string roleName, CancellationToken token)
    {
        return await _distributionDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, token);
    }

    public bool IsRoleInDb(Role? role)
    {
        return role is not null;
    }

    public AppUser? CreateNewAppUser(RegisterRequest request, Tuple<string, string> hash, int roleId)
    {
        return new AppUser()
        {
            Login = request.Login,
            Password = hash.Item1,
            Salt = hash.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(2),
            RoleId = roleId
        };
    }

    public async Task<int> AddNewUserAsync(AppUser? user, CancellationToken token)
    {
        await _distributionDbContext.AppUsers.AddAsync(user, token);
        await _distributionDbContext.SaveChangesAsync(token);
        return 0;
    }

    public async Task<AppUser?> GetUserAsync(string login, CancellationToken token)
    {
        return await _distributionDbContext.AppUsers.Include(u => u.Role).Where(u => u.Login == login).FirstOrDefaultAsync(token);
    }
    
    public SymmetricSecurityKey NewSymmetricSecurityKey()
    {
        var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
    }

    public SigningCredentials NewSigningCredentials(SymmetricSecurityKey key)
    {
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public JwtSecurityToken NewJwtSecurityToken(Claim[] claims, SigningCredentials credentials)
    {
        return new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials
            );
    }

    public async Task<int> RefreshTokensAsync(AppUser user,CancellationToken token)
    {
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        await _distributionDbContext.SaveChangesAsync(token);
        return 0;
    }

    public string Response(JwtSecurityToken token, AppUser user)
    {
        return new
        {
            accesToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        }.ToString();
    }

    public async Task<AppUser?> GetUserAsyncByToken(string refreshToken, CancellationToken token)
    {
        return await _distributionDbContext.AppUsers.Include(u => u.Role).Where(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync(token);
    }

    public bool IsRefreshTokenExpired(DateTime? date)
    {
        return date < DateTime.Now;
    }
}