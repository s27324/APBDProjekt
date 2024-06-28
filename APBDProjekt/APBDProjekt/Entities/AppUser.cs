using APBDProjekt.Entities.Configs;

namespace APBDProjekt.Entities;

public class AppUser
{
    public int UserId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
}