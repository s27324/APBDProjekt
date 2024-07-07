using System.Transactions;
using APBDProjekt.DTOs.Client;
using APBDProjekt.Entities;
using APBDProjekt.Repositories;

namespace APBDProjekt.Services;

public class ClientService: IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public bool IsPESELTheRightSize(string PESEL)
    {
        return _clientRepository.IsPESELTheRightSize(PESEL);
    }

    public bool IsPESELOnlyDigits(string PESEL)
    {
        return _clientRepository.IsPESELOnlyDigits(PESEL);
    }

    public int DoPhoneNumberHaveAreaCode(string phoneNumber)
    {
        return _clientRepository.DoPhoneNumberHaveAreaCode(phoneNumber);
    }

    public int EmailAddressValid(string email)
    {
        return _clientRepository.EmailAddressValid(email);
    }

    public bool IsOnlyLetters(string word)
    {
        return _clientRepository.IsOnlyLetters(word);
    }

    public bool IsEmptyOrWhiteSpace(string word)
    {
        return _clientRepository.IsEmptyOrWhiteSpace(word);
    }

    public async Task<bool> PrivateClientExistsAsync(string PESEL, CancellationToken token)
    {
        return await _clientRepository.PrivateClientExistsAsync(PESEL, token);
    }

    public async Task<int> AddingInTransactionPrivateAsync(PrivateClientDTO client, CancellationToken token)
    {
        return await _clientRepository.AddingInTransactionPrivateAsync(client, token);
    }

    public async Task<string> AddNewPrivateClientAndClientAsync(PrivateClientDTO client, CancellationToken token)
    {
        int phoneNumberDecision = DoPhoneNumberHaveAreaCode(client.phoneNumber);

        switch (phoneNumberDecision)
        {
            case -1:
                throw new Exception("Error: Phone number cannot be null");
            case -2:
                throw new Exception("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits");
        }

        int emailAddressDecision = EmailAddressValid(client.email);
        switch (emailAddressDecision)
        {
            case -1:
                throw new Exception("Error: Email is invalid. Please remember about @somemail.someending.");
            case -2:
                throw new Exception("Error: Email address cannot be empty.");
        }

        if (IsEmptyOrWhiteSpace(client.firstName))
        {
            throw new Exception("Error: Client's firstname cannot be empty or whitespace");
        }
        if (!IsOnlyLetters(client.firstName))
        {
            throw new Exception("Error: Client's firstname should be only made from letters.");
        }
        
        if (IsEmptyOrWhiteSpace(client.lastName))
        {
            throw new Exception("Error: Client's lastname cannot be empty or whitespace");
        }
        if (!IsOnlyLetters(client.lastName))
        {
            throw new Exception("Error: Client's lastname should be only made from letters.");
        }
        if (IsEmptyOrWhiteSpace(client.address))
        {
            throw new Exception("Error: Client's address cannot be empty or whitespace.");
        }
        
        if (!IsPESELOnlyDigits(client.PESEL))
        {
            throw new Exception("Error: PESEL should only consist of digits.");
        }
        
        if (!IsPESELTheRightSize(client.PESEL))
        {
            throw new Exception("Error: PESEL should be exactly 11 digits long.");
        }

        if (await PrivateClientExistsAsync(client.PESEL, token))
        {
            throw new Exception($"Error: Private client with PESEL: {client.PESEL} already exists.");
        }

        int resultOfTrans = await AddingInTransactionPrivateAsync(client, token);

        if (resultOfTrans == -1)
        {
            throw new Exception("Error: Transaction rollbacked.");
        }

        return $"Succesffully added new private client with id {resultOfTrans}.";
    }

    public bool IsKRSTheRightSize(string KRS)
    {
        return _clientRepository.IsKRSTheRightSize(KRS);
    }

    public bool IsKRSOnlyDigits(string KRS)
    {
        return _clientRepository.IsKRSOnlyDigits(KRS);
    }

    public async Task<bool> CompanyClientExistsAsync(string KRS, CancellationToken token)
    {
        return await _clientRepository.CompanyClientExistsAsync(KRS, token);
    }

    public async Task<int> AddingInTransactionCompanyAsync(CompanyClientDTO client, CancellationToken token)
    {
        return await _clientRepository.AddingInTransactionCompanyAsync(client, token);
    }

    public async Task<string> AddNewCompanyAndClientAsync(CompanyClientDTO client, CancellationToken token)
    {
        int phoneNumberDecision = DoPhoneNumberHaveAreaCode(client.phoneNumber);

        switch (phoneNumberDecision)
        {
            case -1:
                throw new Exception("Error: Phone number cannot be null");
            case -2:
                throw new Exception("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits");
        }

        int emailAddressDecision = EmailAddressValid(client.email);
        switch (emailAddressDecision)
        {
            case -1:
                throw new Exception("Error: Email is invalid. Please remember about @somemail.someending");
            case -2:
                throw new Exception("Error: Email address cannot be empty.");
        }
        
        if (IsEmptyOrWhiteSpace(client.name))
        {
            throw new Exception("Error: Company's name cannot be empty or whitespace");
        }
        
        if (IsEmptyOrWhiteSpace(client.address))
        {
            throw new Exception("Error: Client's address cannot be empty or whitespace.");
        }
        
        if (!IsKRSOnlyDigits(client.KRS))
        {
            throw new Exception("Error: KRS should only consist of digits.");
        }

        if (!IsKRSTheRightSize(client.KRS))
        {
            throw new Exception("Error: KRS should be exactly 9 or 14 digits long.");
        }
        
        if (await CompanyClientExistsAsync(client.KRS, token))
        {
            throw new Exception($"Error: Company client with KRS: {client.KRS} already exists.");
        }
        
        int resultOfTrans = await AddingInTransactionCompanyAsync(client, token);

        if (resultOfTrans == -1)
        {
            throw new Exception("Error: Transaction rollbacked.");
        }

        return $"Succesffully added new company client with id {resultOfTrans}.";
    }

    
    
    public async Task<bool> IsPrivateClientInDbAsync(int id, CancellationToken token)
    {
        return await _clientRepository.IsPrivateClientInDbAsync(id, token);
    }

    public async Task<Client> GetClientAsync(int id, CancellationToken token)
    {
        return await _clientRepository.GetClientAsync(id, token);
    }

    public async Task<int> SoftDeletePrivateClientAsync(Client client, CancellationToken token)
    {
        return await _clientRepository.SoftDeletePrivateClientAsync(client, token);
    }

    public async Task<string> DeletePrivateClientAsync(int id, CancellationToken token)
    {
        if (!await IsPrivateClientInDbAsync(id, token))
        {
            throw new Exception($"Error: Private client with id {id} not found or had been previously deleted.");
        }

        Client client = await GetClientAsync(id, token);

        await SoftDeletePrivateClientAsync(client, token);
        return $"Successfully deleted private client with id {id}.";
    }

    public async Task<int> UpdatePrivateClientAsync(UpdatePrivateClientDTO updateClient, Client client, CancellationToken token)
    {
        return await _clientRepository.UpdatePrivateClientAsync(updateClient, client, token);
    }

    public async Task<string> UpdatePrivateClientInfoAsync(int clientId, UpdatePrivateClientDTO client, CancellationToken token)
    {
        if (!await IsPrivateClientInDbAsync(clientId, token))
        {
            throw new Exception($"Error: Private client with id {clientId} not found or had been previously deleted.");
        }
        
        if (!string.IsNullOrWhiteSpace(client.firstName))
        {
            if (!IsOnlyLetters(client.firstName))
            {
                throw new Exception("Error: Client's firstname should be only made from letters.");
            }
        }
        if (!string.IsNullOrWhiteSpace(client.lastName))
        {
            if (!IsOnlyLetters(client.lastName))
            {
                throw new Exception("Error: Client's lastname should be only made from letters.");
            }
        }
        if (!string.IsNullOrWhiteSpace(client.email))
        {
            int emailAddressDecision = EmailAddressValid(client.email);
            if (emailAddressDecision == -1)
                throw new Exception("Error: Email is invalid. Please remember about @somemail.someending");
        }
        if (!string.IsNullOrWhiteSpace(client.phoneNumber))
        {
            int phoneNumberDecision = DoPhoneNumberHaveAreaCode(client.phoneNumber);

            if (phoneNumberDecision == -2)
                throw new Exception("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits");
            
        }

        Client clientToUpdate = await GetClientAsync(clientId, token);

        int transResult = await UpdatePrivateClientAsync(client, clientToUpdate, token);
        if (transResult == -1)
        {
            throw new Exception("Error: Transaction rollbacked.");
        }

        return $"Successfully updated private client with id {clientId}.";
    }

    public async Task<bool> IsCompanyClientInDbAsync(int id, CancellationToken token)
    {
        return await _clientRepository.IsCompanyClientInDbAsync(id, token);
    }

    public async Task<int> UpdateCompanyClientAsync(UpdateCompanyClientDTO updateClient, Client client, CancellationToken token)
    {
        return await _clientRepository.UpdateCompanyClientAsync(updateClient, client, token);
    }

    public async Task<string> UpdateCompanyClientInfoAsync(int clientId, UpdateCompanyClientDTO client, CancellationToken token)
    {
        if (!await IsCompanyClientInDbAsync(clientId, token))
        {
            throw new Exception($"Error: Company client with id {clientId} not found or had been previously deleted.");
        }
        
        if (!string.IsNullOrWhiteSpace(client.email))
        {
            int emailAddressDecision = EmailAddressValid(client.email);
            if (emailAddressDecision == -1)
                throw new Exception("Error: Email is invalid. Please remember about @somemail.someending");
        }
        if (!string.IsNullOrWhiteSpace(client.phoneNumber))
        {
            int phoneNumberDecision = DoPhoneNumberHaveAreaCode(client.phoneNumber);

            if (phoneNumberDecision == -2)
                throw new Exception("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits");
            
        }
        
        Client clientToUpdate = await GetClientAsync(clientId, token);
        
        int transResult = await UpdateCompanyClientAsync(client, clientToUpdate, token);
        if (transResult == -1)
        {
            throw new Exception("Error: Transaction rollbacked.");
        }

        return $"Successfully updated company client with id {clientId}.";
    }
}