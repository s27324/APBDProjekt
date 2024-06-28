using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class AppUserEfConfiguration: IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder
            .HasKey(u => u.UserId)
            .HasName("User_pk");
        builder
            .Property(u => u.UserId)
            .UseIdentityColumn();
        builder
            .Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(70);
        builder
            .Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(70);
        builder
            .Property(u => u.Salt)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(u => u.RefreshToken)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(u => u.RefreshTokenExp);
        
        builder
            .HasOne(u => u.Role)
            .WithMany(u => u.AppUsers)
            .HasForeignKey(u => u.RoleId)
            .HasConstraintName("AppUser_Role")
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(nameof(AppUser));
    }
}