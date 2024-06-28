using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class CompanyEfConfiguration: IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .HasKey(c => c.CompanyId)
            .HasName("Company_pk");
        builder
            .Property(c => c.CompanyId)
            .UseIdentityColumn();
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(120);
        builder
            .Property(c => c.KRS)
            .IsRequired()
            .HasMaxLength(14);

        builder.ToTable(nameof(Company));

        Company[] companies =
        {
            new Company()
            {
                CompanyId = 1, Name = "Cashzo", KRS = "449188792"
            },
            new Company()
            {
                CompanyId = 2, Name = "Mathico", KRS = "109790978"
            },
            new Company()
            {
                CompanyId = 3, Name = "Lingify", KRS = "66882822454381"
            },
            new Company()
            {
                CompanyId = 4, Name = "Fotofuse", KRS = "668021149"
            }
        };

        builder.HasData(companies);
    }
}