using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class ContractEfConfiguration: IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder
            .HasKey(c => c.ContractId)
            .HasName("Contract_pk");
        builder
            .Property(c => c.ContractId)
            .UseIdentityColumn();
        builder
            .Property(c => c.CurrentCharge)
            .IsRequired()
            .HasPrecision(12, 2);
        builder
            .Property(c => c.MaxCharge)
            .IsRequired()
            .HasPrecision(12, 2);
        builder
            .Property(c => c.StartDate)
            .IsRequired();
        builder
            .Property(c => c.EndDate)
            .IsRequired();
        builder
            .Property(c => c.IsSigned)
            .IsRequired();
        builder
            .Property(c => c.YearsOfSupport)
            .IsRequired();

        builder
            .HasOne(c => c.Client)
            .WithMany(c => c.Contracts)
            .HasForeignKey(c => c.ClientId)
            .HasConstraintName("Contract_Client")
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(c => c.SoftwareSystem)
            .WithMany(s => s.Contracts)
            .HasForeignKey(c => c.SoftwareSystemId)
            .HasConstraintName("Contract_SoftwareSystem")
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(nameof(Contract));

        Contract[] contracts =
        {
            new Contract()
            {
                ContractId = 1, CurrentCharge = new decimal(57000), MaxCharge = new decimal(57000),
                StartDate = new DateTime(2022, 4, 12, 0, 0, 0), EndDate = new DateTime(2022, 4, 22, 0, 0, 0),
                IsSigned = true, YearsOfSupport = 4, SoftwareSystemId = 1, ClientId = 1
            },
            new Contract()
            {
                ContractId = 2, CurrentCharge = new decimal(0), MaxCharge = new decimal(18500),
                StartDate = new DateTime(2021, 7, 1, 0, 0, 0), EndDate = new DateTime(2021, 7, 30, 0, 0, 0),
                IsSigned = false, YearsOfSupport = 2, SoftwareSystemId = 2, ClientId = 2
            },
            new Contract()
            {
                ContractId = 3, CurrentCharge = new decimal(18500), MaxCharge = new decimal(18500),
                StartDate = new DateTime(2021, 8, 1, 0, 0, 0), EndDate = new DateTime(2021, 7, 29, 0, 0, 0),
                IsSigned = true, YearsOfSupport = 1, SoftwareSystemId = 2, ClientId = 2
            },
            new Contract()
            {
                ContractId = 4, CurrentCharge = new decimal(4000), MaxCharge = new decimal(37000),
                StartDate = new DateTime(2024, 6, 26, 0, 0, 0), EndDate = new DateTime(2024, 7, 14, 0, 0, 0),
                IsSigned = false, YearsOfSupport = 3, SoftwareSystemId = 3, ClientId = 5
            },
            new Contract()
            {
                ContractId = 5, CurrentCharge = new decimal(0), MaxCharge = new decimal(68000),
                StartDate = new DateTime(2024, 6, 24, 0, 0, 0), EndDate = new DateTime(2024, 7, 16, 0, 0, 0),
                IsSigned = false, YearsOfSupport = 4, SoftwareSystemId = 6, ClientId = 7
            }
        };

        builder.HasData(contracts);
    }
}