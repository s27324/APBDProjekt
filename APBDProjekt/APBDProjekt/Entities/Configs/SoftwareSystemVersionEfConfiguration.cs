using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class SoftwareSystemVersionEfConfiguration: IEntityTypeConfiguration<SoftwareSystemVersion>
{
    public void Configure(EntityTypeBuilder<SoftwareSystemVersion> builder)
    {
        builder
            .HasKey(ssv => ssv.SoftwareSystemVersionId)
            .HasName("SoftwareSystemVersion_pk");
        builder
            .Property(ssv => ssv.SoftwareSystemVersionId)
            .UseIdentityColumn();
        builder
            .Property(ssv => ssv.ReleaseDate)
            .IsRequired();

        builder
            .HasOne(s => s.SoftwareSystem)
            .WithMany(ssv => ssv.SoftwareSystemVersions)
            .HasForeignKey(s => s.SoftwareSystemId)
            .HasConstraintName("SoftwareSystemVersion_SoftwareSystem")
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(v => v.Version)
            .WithMany(ssv => ssv.SoftwareSystemVersions)
            .HasForeignKey(v => v.VersionId)
            .HasConstraintName("SoftwareSystemVersion_Version")
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("SoftwareSystem_Version");

        SoftwareSystemVersion[] softwareSystemVersions =
        {
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 1,
                SoftwareSystemId = 1, VersionId = 4, ReleaseDate = new DateTime(2019, 10, 1, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 2,
                SoftwareSystemId = 1, VersionId = 3, ReleaseDate = new DateTime(2020, 7, 27, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 3,
                SoftwareSystemId = 1, VersionId = 2, ReleaseDate = new DateTime(2023, 4, 19, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 4,
                SoftwareSystemId = 1, VersionId = 4, ReleaseDate = new DateTime(2024, 6, 26, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 5,
                SoftwareSystemId = 2, VersionId = 3, ReleaseDate = new DateTime(2020, 3, 12, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 6,
                SoftwareSystemId = 2, VersionId = 2, ReleaseDate = new DateTime(2024, 5, 19, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 8, 
                SoftwareSystemId = 3, VersionId = 4, ReleaseDate = new DateTime(2017, 6, 1, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 9, 
                SoftwareSystemId = 3, VersionId = 2, ReleaseDate = new DateTime(2022, 5, 19, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 10, 
                SoftwareSystemId = 4, VersionId = 6, ReleaseDate = new DateTime(2015, 8, 28, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 11, 
                SoftwareSystemId = 4, VersionId = 2, ReleaseDate = new DateTime(2024, 6, 24, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 12, 
                SoftwareSystemId = 5, VersionId = 3, ReleaseDate = new DateTime(2022, 2, 16, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 13, 
                SoftwareSystemId = 5, VersionId = 2, ReleaseDate = new DateTime(2024, 2, 16, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 14, 
                SoftwareSystemId = 6, VersionId = 5, ReleaseDate = new DateTime(2008, 7, 14, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 15, 
                SoftwareSystemId = 6, VersionId = 4, ReleaseDate = new DateTime(2013, 3, 26, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 16, 
                SoftwareSystemId = 6, VersionId = 3, ReleaseDate = new DateTime(2019, 6, 13, 0, 0, 0)
            },
            new SoftwareSystemVersion()
            {
                SoftwareSystemVersionId = 17, 
                SoftwareSystemId = 6, VersionId = 2, ReleaseDate = new DateTime(2023, 1, 13, 0, 0, 0)
            }
        };
            
       

        builder.HasData(softwareSystemVersions);
    }
}