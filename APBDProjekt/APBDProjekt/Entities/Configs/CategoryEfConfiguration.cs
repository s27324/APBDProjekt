using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class CategoryEfConfiguration: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasKey(c => c.CategoryId)
            .HasName("Category_pk");
        builder
            .Property(c => c.CategoryId)
            .UseIdentityColumn();
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.ToTable(nameof(Category));

        Category[] categories =
        {
            new Category()
            {
                CategoryId = 1, Name = "Finance"
            },
            new Category()
            {
                CategoryId = 2, Name = "Education"
            },
            new Category()
            {
                CategoryId = 3, Name = "Utility"
            },
            new Category()
            {
                CategoryId = 4, Name = "Calculation"
            },
            new Category()
            {
                CategoryId = 5, Name = "Translation"
            },
            new Category()
            {
                CategoryId = 6, Name = "Image manipulation"
            }
        };

        builder.HasData(categories);

        builder
            .HasMany(c => c.IdSoftwareSystems)
            .WithMany(s => s.IdCategories)
            .UsingEntity<Dictionary<string, object>>(
                "SoftwareSystem_Category",
                s => s.HasOne<SoftwareSystem>().WithMany().HasForeignKey("SoftwareSystemId")
                    .OnDelete(DeleteBehavior.Restrict),
                c => c.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.Restrict))
            .HasData(
                new { SoftwareSystemId = 1, CategoryId = 1 },
                new { SoftwareSystemId = 1, CategoryId = 2 },
                new { SoftwareSystemId = 1, CategoryId = 4 },
                new { SoftwareSystemId = 2, CategoryId = 2 },
                new { SoftwareSystemId = 2, CategoryId = 4 },
                new { SoftwareSystemId = 3, CategoryId = 1 },
                new { SoftwareSystemId = 3, CategoryId = 3 },
                new { SoftwareSystemId = 4, CategoryId = 2 },
                new { SoftwareSystemId = 4, CategoryId = 4 },
                new { SoftwareSystemId = 5, CategoryId = 5 },
                new { SoftwareSystemId = 6, CategoryId = 6 }
                );
    }
}