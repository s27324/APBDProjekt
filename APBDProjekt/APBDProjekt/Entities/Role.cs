namespace APBDProjekt.Entities.Configs;

public class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
}