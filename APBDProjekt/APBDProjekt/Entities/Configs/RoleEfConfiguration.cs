using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class RoleEfConfiguration: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .HasKey(r => r.RoleId)
            .HasName("AppUser_pk");
        builder
            .Property(r => r.RoleId)
            .UseIdentityColumn();
        builder
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(70);

        builder.ToTable(nameof(Role));

        Role[] roles =
        {
            new Role()
            {
                RoleId = 1, Name = "Employee"
            },
            new Role()
            {
                RoleId = 2, Name = "Admin"
            }
        };

        builder.HasData(roles);
    }
}