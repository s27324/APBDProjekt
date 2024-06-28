using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class SoftwareSystemEfConfiguration: IEntityTypeConfiguration<SoftwareSystem>
{
    public void Configure(EntityTypeBuilder<SoftwareSystem> builder)
    {
        builder
            .HasKey(s => s.SoftwareSystemId)
            .HasName("SoftwareSystem_pk");
        builder
            .Property(s => s.SoftwareSystemId)
            .UseIdentityColumn();
        builder
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(70);
        builder
            .Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(300);
        builder
            .Property(s => s.Price)
            .IsRequired()
            .HasPrecision(12, 2);

        builder.ToTable(nameof(SoftwareSystem));

        SoftwareSystem[] softwareSystems =
        {
            new SoftwareSystem()
            {
                SoftwareSystemId = 1, Name = "FinanceExpert",
                Description =
                    "System to help companies with all financial issues, such as revenue, and to teach inexperienced employees.", 
                Price = 54000
            },
            new SoftwareSystem()
            {
                SoftwareSystemId = 2, Name = "MathsHelper",
                Description =
                    "System to help students understand difficult mathematical topics.", 
                Price = 17500
            },
            new SoftwareSystem()
            {
                SoftwareSystemId = 3, Name = "WageManager",
                Description =
                    "System to help count the paychecks of employees from different departments based on hours worked and other factors.", 
                Price = 35000
            },
            new SoftwareSystem()
            {
                SoftwareSystemId = 4, Name = "Calculator++",
                Description =
                    "System that helps calculate very large numbers and complex data.", 
                Price = 15000
            },
            new SoftwareSystem()
            {
                SoftwareSystemId = 5, Name = "TranslatorMax",
                Description =
                    "System to improve the tasks of the translator. In addition, equipped with a powerful database of words, synonyms, etc.", 
                Price = 44000
            },
            new SoftwareSystem()
            {
                SoftwareSystemId = 6, Name = "PixelPerfect",
                Description =
                    "System to help you transform and enhance your photos with intuitive, professional editing tools.", 
                Price = 65000
            }
        };

        builder.HasData(softwareSystems);
    }
}