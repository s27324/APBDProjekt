using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class VersionEfConfiguration: IEntityTypeConfiguration<Version>
{
    public void Configure(EntityTypeBuilder<Version> builder)
    {
        builder
            .HasKey(v => v.VersionId)
            .HasName("Version_pk");
        builder
            .Property(v => v.VersionId)
            .UseIdentityColumn();
        builder
            .Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder
            .Property(v => v.VersionDate)
            .IsRequired();

        builder.ToTable(nameof(Version));

        Version[] versions =
        {
            new Version()
            {
                VersionId = 1, Name = "1.1"
            },
            new Version()
            {
                VersionId = 2, Name = "1.0"
            },
            new Version()
            {
                VersionId = 3, Name = "0.9"
            },
            new Version()
            {
                VersionId = 4, Name = "0.8"
            },
            new Version()
            {
                VersionId = 5, Name = "0.7"
            },
            new Version()
            {
                VersionId = 6, Name = "0.6"
            }
        };

        builder.HasData(versions);
    }
}