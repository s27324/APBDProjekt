using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class DiscountEfConfiguration: IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder
            .HasKey(d => d.DiscountId)
            .HasName("Discount_pk");
        builder
            .Property(d => d.DiscountId)
            .UseIdentityColumn();
        builder
            .Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder
            .Property(d => d.Offer)
            .IsRequired()
            .HasMaxLength(150);
        builder
            .Property(d => d.Value)
            .IsRequired()
            .HasPrecision(5, 2);
        builder
            .Property(d => d.Timeslot)
            .IsRequired()
            .HasMaxLength(20);

        builder.ToTable(nameof(Discount));

        Discount[] discounts =
        {
            new Discount()
            {
                DiscountId = 1, Name = "The beginning of the school year Discount",
                Offer = "Discount on education systems", Value = new decimal(10), Timeslot = "01-08 30-09"
            },
            new Discount()
            {
                DiscountId = 2, Name = "New Year Discount", Offer = "Discount on all systems", Value = new decimal(5.5),
                Timeslot = "01-01 16-01"
            },
            new Discount()
            {
                DiscountId = 3, Name = "Finance February Discount", Offer = "Discount on finance systems",
                Value = new decimal(7.5), Timeslot = "01-02 25-02"
            },
            new Discount()
            {
                DiscountId = 4, Name = "Utility Holiday Discount", Offer = "Discount on utility systems",
                Value = new decimal(12.5), Timeslot = "01-07 01-09"
            },
            new Discount()
            {
                DiscountId = 5, Name = "Calculation Systems Discount", Offer = "Discount on calculation systems",
                Value = new decimal(8.5), Timeslot = "13-01 14-03"
            },
            new Discount()
            {
                DiscountId = 6, Name = "Learning and finance Discount",
                Offer = "Discount on finance and education systems", Value = new decimal(6.5), Timeslot = "20-02 06-08"
            }
        };

        builder.HasData(discounts);
        
        builder
            .HasMany(d => d.IdSoftwareSystems)
            .WithMany(s => s.IdDiscounts)
            .UsingEntity<Dictionary<string, object>>(
                "SoftwareSystem_Discount",
                s => s.HasOne<SoftwareSystem>().WithMany().HasForeignKey("SoftwareSystemId")
                    .OnDelete(DeleteBehavior.Restrict),
                d => d.HasOne<Discount>().WithMany().HasForeignKey("DiscountId").OnDelete(DeleteBehavior.Restrict))
            .HasData(
                new { SoftwareSystemId = 1, DiscountId = 1 },
                new { SoftwareSystemId = 1, DiscountId = 2 },
                new { SoftwareSystemId = 1, DiscountId = 3 },
                new { SoftwareSystemId = 1, DiscountId = 5 },
                new { SoftwareSystemId = 1, DiscountId = 6 },
                new { SoftwareSystemId = 2, DiscountId = 1 },
                new { SoftwareSystemId = 2, DiscountId = 2 },
                new { SoftwareSystemId = 2, DiscountId = 5 },
                new { SoftwareSystemId = 2, DiscountId = 6 },
                new { SoftwareSystemId = 3, DiscountId = 2 },
                new { SoftwareSystemId = 3, DiscountId = 3 },
                new { SoftwareSystemId = 3, DiscountId = 4 },
                new { SoftwareSystemId = 4, DiscountId = 1 },
                new { SoftwareSystemId = 4, DiscountId = 2 },
                new { SoftwareSystemId = 4, DiscountId = 5 },
                new { SoftwareSystemId = 4, DiscountId = 6 },
                new { SoftwareSystemId = 5, DiscountId = 2 },
                new { SoftwareSystemId = 6, DiscountId = 2 }
            );
    }
}