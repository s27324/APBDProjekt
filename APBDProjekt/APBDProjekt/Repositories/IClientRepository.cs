using APBDProjekt.DTOs.Client;
using APBDProjekt.Entities;

namespace APBDProjekt.Repositories;

public interface IClientRepository
{
    public bool IsPESELTheRightSize(string PESEL);
    public bool IsPESELOnlyDigits(string PESEL);
    public int DoPhoneNumberHaveAreaCode(string phoneNumber);
    public int EmailAddressValid(string email);
    public bool IsOnlyLetters(string word);
    public bool IsEmptyOrWhiteSpace(string word);
    public Task<bool> PrivateClientExistsAsync(string PESEL, CancellationToken token);
    public Task<int> AddNewPrivateClientAsync(PrivateClientDTO client, CancellationToken token);
    public Task<int> AddNewClientAsync(PrivateClientDTO client, int pClientId,CancellationToken token);
    public Task<int> AddingInTransactionPrivateAsync(PrivateClientDTO client, CancellationToken token);
    public bool IsKRSTheRightSize(string KRS);
    public bool IsKRSOnlyDigits(string KRS);
    public Task<bool> CompanyClientExistsAsync(string KRS, CancellationToken token);
    public Task<int> AddNewCompanyClientAsync(CompanyClientDTO client, CancellationToken token);
    public Task<int> AddNewClientAsync(CompanyClientDTO client, int cClientId, CancellationToken token);
    public Task<int> AddingInTransactionCompanyAsync(CompanyClientDTO client, CancellationToken token);


    public Task<bool> IsPrivateClientInDbAsync(int id, CancellationToken token);
    public Task<Client> GetClientAsync(int id, CancellationToken token);
    public Task<int> SoftDeletePrivateClientAsync(Client client, CancellationToken token);

    
    public Task<int> UpdatePrivateClientAsync(UpdatePrivateClientDTO updateClient, Client client, CancellationToken token);


    public Task<bool> IsCompanyClientInDbAsync(int id, CancellationToken token);
    public Task<int> UpdateCompanyClientAsync(UpdateCompanyClientDTO updateClient, Client client, CancellationToken token);
}