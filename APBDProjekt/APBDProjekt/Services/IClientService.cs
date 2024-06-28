using APBDProjekt.DTOs.Client;
using APBDProjekt.Entities;

namespace APBDProjekt.Services;

public interface IClientService
{
    public bool IsPESELTheRightSize(string PESEL);
    public bool IsPESELOnlyDigits(string PESEL);
    public int DoPhoneNumberHaveAreaCode(string phoneNumber);
    public int EmailAddressValid(string email);
    public bool IsOnlyLetters(string word);
    public bool IsEmptyOrWhiteSpace(string word);
    public Task<bool> PrivateClientExistsAsync(string PESEL, CancellationToken token);
    public Task<int> AddingInTransactionPrivateAsync(PrivateClientDTO client, CancellationToken token);
    public Task<string> AddNewPrivateClientAndClientAsync(PrivateClientDTO client, CancellationToken token);
    public bool IsKRSTheRightSize(string KRS);
    public bool IsKRSOnlyDigits(string KRS);
    public Task<bool> CompanyClientExistsAsync(string KRS, CancellationToken token);
    public Task<int> AddingInTransactionCompanyAsync(CompanyClientDTO client, CancellationToken token);
    public Task<string> AddNewCompanyAndClientAsync(CompanyClientDTO client, CancellationToken token);
    
    
    public Task<bool> IsPrivateClientInDbAsync(int id, CancellationToken token);
    public Task<Client> GetClientAsync(int id, CancellationToken token);
    public Task<int> SoftDeletePrivateClientAsync(Client client, CancellationToken token);
    public Task<string> DeletePrivateClientAsync(int id, CancellationToken token);


    public Task<int> UpdatePrivateClientAsync(UpdatePrivateClientDTO updateClient, Client client, CancellationToken token);
    public Task<string> UpdatePrivateClientInfoAsync(int clientId, UpdatePrivateClientDTO client, CancellationToken token);
    
    
    public Task<bool> IsCompanyClientInDbAsync(int id, CancellationToken token);
    public Task<int> UpdateCompanyClientAsync(UpdateCompanyClientDTO updateClient, Client client, CancellationToken token);
    public Task<string> UpdateCompanyClientInfoAsync(int clientId, UpdateCompanyClientDTO client, CancellationToken token);
}