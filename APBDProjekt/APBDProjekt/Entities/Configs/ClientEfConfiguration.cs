using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBDProjekt.Entities.Configs;

public class ClientEfConfiguration: IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder
            .HasKey(c => c.ClientId)
            .HasName("Client_pk");
        builder
            .Property(c => c.ClientId)
            .UseIdentityColumn();
        builder
            .Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(150);
        builder
            .Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(50);
        builder
            .Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
        builder
            .Property(c => c.IsDeleted)
            .IsRequired();
        
        builder
            .HasOne(c => c.Company)
            .WithMany(c => c.Clients)
            .HasForeignKey(c => c.CompanyId)
            .HasConstraintName("Client_Company")
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(c => c.PrivateClient)
            .WithMany(p => p.Clients)
            .HasForeignKey(c => c.PrivateClientId)
            .HasConstraintName("Client_PrivateClient")
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(nameof(Client));

        Client[] clients =
        {
            new Client()
            {
                ClientId = 1, Address = "ul. Chmiel 47c - Zgierz, LU / 65-239", Email = "doe@gmail.com",
                PhoneNumber = "+48 543772365", PrivateClientId = 1, CompanyId = null, IsDeleted = true
            },
            new Client()
            {
                ClientId = 2, Address = "ul. Cybulski 81 - Ścinawa, PK / 74-648", Email = "marltom@gmail.com",
                PhoneNumber = "+1 9792862590", PrivateClientId = 2, CompanyId = null, IsDeleted = false
            },
            new Client()
            {
                ClientId = 3, Address = "ul. Śliwa 4/1 - Wieruszów, LU / 99-878", Email = "soniaduda@gmail.com",
                PhoneNumber = "+48 852951976", PrivateClientId = 3, CompanyId = null, IsDeleted = false
            },
            new Client()
            {
                ClientId = 4, Address = "al. Kostrzewa 270 - Lipiany, LB / 71-013", Email = "tadbaran@gmail.com",
                PhoneNumber = "+48 904095402", PrivateClientId = 4, CompanyId = null, IsDeleted = false
            },
            new Client()
            {
                ClientId = 5, Address = "pl. Szczepański 32c - Jeziorany, KP / 52-673", Email = "dir@cashzo.com",
                PhoneNumber = "+48 802107161", PrivateClientId = null, CompanyId = 1, IsDeleted = false
            },
            new Client()
            {
                ClientId = 6, Address = "pl. Turek 05c - Polanów, ZP / 07-186", Email = "info@mathico.com",
                PhoneNumber = "+48 209770172", PrivateClientId = null, CompanyId = 2, IsDeleted = false
            },
            new Client()
            {
                ClientId = 7, Address = "42 Ockham Road - East Witton, DL8 8TU", Email = "sales@lingify.uk",
                PhoneNumber = "+44 7994365813", PrivateClientId = null, CompanyId = 3, IsDeleted = false
            },
            new Client()
            {
                ClientId = 8, Address = "pl. Molenda 42c - Gdynia, LD / 86-752", Email = "fotofuse@gmail.com",
                PhoneNumber = "+48 517455585", PrivateClientId = null, CompanyId = 4, IsDeleted = false
            }
        };

        builder.HasData(clients);
    }
}