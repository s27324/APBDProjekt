using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class PrivateClientEfConfiguration: IEntityTypeConfiguration<PrivateClient>
{
    public void Configure(EntityTypeBuilder<PrivateClient> builder)
    {
        builder
            .HasKey(p => p.PrivateClientId)
            .HasName("PrivateClient_pk");
        builder
            .Property(p => p.PrivateClientId)
            .UseIdentityColumn();
        builder
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        builder
            .Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(50);
        builder
            .Property(p => p.PESEL)
            .IsRequired()
            .HasMaxLength(11);

        builder.ToTable(nameof(PrivateClient));

        PrivateClient[] privateClients =
        {
            new PrivateClient()
            {
                PrivateClientId = 1, FirstName = "John", LastName = "Doe", PESEL = "89110281190"
            },
            new PrivateClient()
            {
                PrivateClientId = 2, FirstName = "Marlena", LastName = "Tomczyk", PESEL = "43071918403"
            },
            new PrivateClient()
            {
                PrivateClientId = 3, FirstName = "Sonia", LastName = "Duda", PESEL = "76102295729"
            },
            new PrivateClient()
            {
                PrivateClientId = 4, FirstName = "Tadeusz", LastName = "Baran", PESEL = "33110448277"
            }
        };

        builder.HasData(privateClients);
    }
}