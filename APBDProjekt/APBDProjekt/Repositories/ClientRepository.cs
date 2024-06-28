using System.Net.Mail;
using System.Text.RegularExpressions;
using APBDProjekt.DTOs.Client;
using APBDProjekt.Entities;
using Microsoft.EntityFrameworkCore;

namespace APBDProjekt.Repositories;

public class ClientRepository: IClientRepository
{
    private readonly SoftwareDistributionDbContext _distributionDbContext;

    public ClientRepository(SoftwareDistributionDbContext distributionDbContext)
    {
        _distributionDbContext = distributionDbContext;
    }

    public bool IsPESELTheRightSize(string PESEL)
    {
        return PESEL.Length == 11;
    }

    public bool IsPESELOnlyDigits(string PESEL)
    {
        return PESEL.All(char.IsDigit);
    }

    public int DoPhoneNumberHaveAreaCode(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return -1;
        }

        string pattern = @"^\+\d{1,3} \d{9,11}$";
        var regex = new Regex(pattern);
        if (regex.IsMatch(phoneNumber))
        {
            return 0;
        }

        return -2;
    }

    public int EmailAddressValid(string email)
    {
        if (email.Length == 0)
        {
            return -2;
        }
        try
        {
            var mail = new MailAddress(email);
            return 0;
        }
        catch (FormatException)
        {
            return -1;
        }
    }

    public bool IsOnlyLetters(string word)
    {
        string[] splited = word.Split(" ");

        foreach (string s in splited)
        {
            bool dec = Regex.IsMatch(s, @"^[a-zA-Z]+$");
            if (dec == false)
            {
                return dec;
            }
        }

        return true;
    }

    public bool IsEmptyOrWhiteSpace(string word)
    {
        return string.IsNullOrWhiteSpace(word);
    }

    public async Task<bool> PrivateClientExistsAsync(string PESEL, CancellationToken token)
    {
        return await _distributionDbContext.PrivateClients.AnyAsync(p => p.PESEL == PESEL, token);
    }
    
    public async Task<int> AddNewPrivateClientAsync(PrivateClientDTO client, CancellationToken token)
    {
        await _distributionDbContext.PrivateClients.AddAsync(new PrivateClient
        {
            FirstName = client.firstName,
            LastName = client.lastName,
            PESEL = client.PESEL
        }, token);
        await _distributionDbContext.SaveChangesAsync(token);

        int newlyAddedPClientId = await _distributionDbContext.PrivateClients.Where(p => p.PESEL == client.PESEL)
            .Select(p => p.PrivateClientId).FirstOrDefaultAsync(token);
        return newlyAddedPClientId;
    }

    public async Task<int> AddNewClientAsync(PrivateClientDTO client, int pClientId, CancellationToken token)
    {
        await _distributionDbContext.Clients.AddAsync(new Client
        {
            Address = client.address,
            Email = client.email,
            PhoneNumber = client.phoneNumber,
            IsDeleted = false,
            PrivateClientId = pClientId,
            CompanyId = null
        }, token);
        await _distributionDbContext.SaveChangesAsync(token);

        int newlyAddedClientId = await _distributionDbContext.Clients.Where(c => c.PrivateClientId == pClientId)
            .Select(c => c.ClientId).FirstOrDefaultAsync(token);
        return newlyAddedClientId;
    }

    public async Task<int> AddingInTransactionPrivateAsync(PrivateClientDTO client,  CancellationToken token)
    {
        using var transaction = await _distributionDbContext.Database.BeginTransactionAsync(token);
        try
        {
            int pClientId = await AddNewPrivateClientAsync(client, token);
            int clientId = await AddNewClientAsync(client, pClientId, token);

            await transaction.CommitAsync(token);
            return clientId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(token);
            return -1;
        }
    }

    public bool IsKRSTheRightSize(string KRS)
    {
        return KRS.Length == 9 || KRS.Length == 14;
    }

    public bool IsKRSOnlyDigits(string KRS)
    {
        return KRS.All(char.IsDigit);
    }

    public async Task<bool> CompanyClientExistsAsync(string KRS, CancellationToken token)
    {
        return await _distributionDbContext.Companies.AnyAsync(c => c.KRS == KRS, token);
    }

    public async Task<int> AddNewCompanyClientAsync(CompanyClientDTO client, CancellationToken token)
    {
        await _distributionDbContext.Companies.AddAsync(new Company()
        {
            Name = client.name,
            KRS = client.KRS
        }, token);
        await _distributionDbContext.SaveChangesAsync(token);

        int newlyAddedCClientId = await _distributionDbContext.Companies.Where(p => p.KRS == client.KRS)
            .Select(p => p.CompanyId).FirstOrDefaultAsync(token);
        return newlyAddedCClientId;
    }

    public async Task<int> AddNewClientAsync(CompanyClientDTO client, int cClientId, CancellationToken token)
    {
        await _distributionDbContext.Clients.AddAsync(new Client
        {
            Address = client.address,
            Email = client.email,
            PhoneNumber = client.phoneNumber,
            IsDeleted = false,
            PrivateClientId = null,
            CompanyId = cClientId
        }, token);
        await _distributionDbContext.SaveChangesAsync(token);

        int newlyAddedClientId = await _distributionDbContext.Clients.Where(c => c.CompanyId == cClientId)
            .Select(c => c.ClientId).FirstOrDefaultAsync(token);
        return newlyAddedClientId;
    }

    public async Task<int> AddingInTransactionCompanyAsync(CompanyClientDTO client, CancellationToken token)
    {
        using var transaction = await _distributionDbContext.Database.BeginTransactionAsync(token);
        try
        {
            int cClientId = await AddNewCompanyClientAsync(client, token);
            int clientId = await AddNewClientAsync(client, cClientId, token);

            await transaction.CommitAsync(token);
            return clientId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(token);
            return -1;
        }
    }
    
    

    public async Task<bool> IsPrivateClientInDbAsync(int id, CancellationToken token)
    {
        return await _distributionDbContext.Clients.AnyAsync(c => c.ClientId == id && c.IsDeleted == false && c.CompanyId == null, token);
    }

    public async Task<Client> GetClientAsync(int id, CancellationToken token)
    {
        return await _distributionDbContext.Clients.Include(c => c.PrivateClient).Include(c => c.Company).SingleAsync(c => c.ClientId == id, token);
    }

    public async Task<int> SoftDeletePrivateClientAsync(Client client, CancellationToken token)
    {
        client.IsDeleted = true;
        await _distributionDbContext.SaveChangesAsync(token);
        return 0;
    }

    public async Task<int> UpdatePrivateClientAsync(UpdatePrivateClientDTO updateClient, Client client, CancellationToken token)
    {
        using var transaction = await _distributionDbContext.Database.BeginTransactionAsync(token);
        try
        {
            if (!string.IsNullOrWhiteSpace(updateClient.firstName))
            {
                client.PrivateClient.FirstName = updateClient.firstName;
            }
            if (!string.IsNullOrWhiteSpace(updateClient.lastName))
            {
                client.PrivateClient.LastName = updateClient.lastName;
            }
            if (!string.IsNullOrWhiteSpace(updateClient.address))
            {
                client.Address = updateClient.address;
            }
            if (!string.IsNullOrWhiteSpace(updateClient.email))
            {
                client.Email = updateClient.email;
            }
            if (!string.IsNullOrWhiteSpace(updateClient.phoneNumber))
            {
                client.PhoneNumber = updateClient.phoneNumber;
            }
            await _distributionDbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            return 0;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(token);
            return -1;
        }
    }

    public async Task<bool> IsCompanyClientInDbAsync(int id, CancellationToken token)
    {
        return await _distributionDbContext.Clients.AnyAsync(c => c.ClientId == id && c.IsDeleted == false && c.PrivateClientId == null, token);
    }

    public async Task<int> UpdateCompanyClientAsync(UpdateCompanyClientDTO updateClient, Client client, CancellationToken token)
    {
        using var transaction = await _distributionDbContext.Database.BeginTransactionAsync(token);
        try
        {
            if (!string.IsNullOrWhiteSpace(updateClient.name))
            {
                client.Company.Name = updateClient.name;
            }
            if (!string.IsNullOrWhiteSpace(updateClient.address))
            {
                client.Address = updateClient.address;
            }
            if (!string.IsNullOrWhiteSpace(updateClient.email))
            {
                client.Email = updateClient.email;
            }
            if (!string.IsNullOrWhiteSpace(updateClient.phoneNumber))
            {
                client.PhoneNumber = updateClient.phoneNumber;
            }
            await _distributionDbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            return 0;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(token);
            return -1;
        }
    }
}