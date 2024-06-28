using APBDProjekt.DTOs.Contract;
using APBDProjekt.Entities;
using APBDProjekt.Repositories;

namespace APBDProjekt.Services;

public class ContractService: IContractService
{
    private readonly IContractRepository _contractRepository;

    public ContractService(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<bool> IsSoftwareSystemInDbAsync(int id, CancellationToken token)
    {
        return await _contractRepository.IsSoftwareSystemInDbAsync(id, token);
    }

    public async Task<bool> IsClientInDbAsync(int id, CancellationToken token)
    {
        return await _contractRepository.IsClientInDbAsync(id, token);
    }

    public async Task<bool> DoesClientHaveContractOnThatSoft(int clientId, int softwareSystemId, CancellationToken token)
    {
        return await _contractRepository.DoesClientHaveContractOnThatSoft(clientId, softwareSystemId, token);
    }

    public bool IsYearsOfSupportCorrect(int years)
    {
        return _contractRepository.IsYearsOfSupportCorrect(years);
    }

    public bool IsDaysForSigningCorrect(int days)
    {
        return _contractRepository.IsDaysForSigningCorrect(days);
    }

    public DateTime GetEndDate(DateTime startDate, int days)
    {
        return _contractRepository.GetEndDate(startDate, days);
    }

    public async Task<decimal> GetBasePriceAsync(int soft, CancellationToken token)
    {
        return await _contractRepository.GetBasePriceAsync(soft, token);
    }

    public decimal GetMaxDiscount(int soft, DateTime date)
    {
        return _contractRepository.GetMaxDiscount(soft, date);
    }

    public async Task<bool> IsPreviousClientAsync(int id, CancellationToken token)
    {
        return await _contractRepository.IsPreviousClientAsync(id, token);
    }

    public decimal CalculateTheFinalPrice(decimal basePrice, decimal discount, decimal additionalDiscount, int yearsOfSupport)
    {
        return _contractRepository.CalculateTheFinalPrice(basePrice, discount, additionalDiscount, yearsOfSupport);
    }

    public async Task<int> AddNewContractAsync(decimal finalPrice, DateTime startDate, DateTime endDate, ContractDTO contract,
        CancellationToken token)
    {
        return await _contractRepository.AddNewContractAsync(finalPrice, startDate, endDate, contract, token);
    }

    public async Task<string> CreateNewContractAsync(ContractDTO contract, CancellationToken token)
    {
        if (!await IsSoftwareSystemInDbAsync(contract.softwareSystemId, token))
        {
            throw new Exception($"Error: Cannot find software system with id: {contract.softwareSystemId}.");
        }
        if (!await IsClientInDbAsync(contract.clientId, token))
        {
            throw new Exception($"Error: Cannot find client with id: {contract.clientId}.");
        }

        if (await DoesClientHaveContractOnThatSoft(contract.clientId, contract.softwareSystemId, token))
        {
            throw new Exception(
                $"Error: Client with id {contract.clientId} already have a contract on software system with id {contract.softwareSystemId}.");
        }

        if (!IsYearsOfSupportCorrect(contract.additionalYearsOfSupport))
        {
            throw new Exception($"Error: Desired software system can additionally supported for only 0, 1, 2 or 3 years.");
        }
        if (!IsDaysForSigningCorrect(contract.daysForSigning))
        {
            throw new Exception(
                "Error: Days to sign should be greater than or equal to 3, but less than or equal to 30.");
        }

        DateTime startDate = DateTime.Today;
        DateTime endDate = GetEndDate(startDate, contract.daysForSigning);
        decimal currentCharge = new decimal(0);
        decimal basePrice = await GetBasePriceAsync(contract.softwareSystemId, token);
        decimal discount = GetMaxDiscount(contract.softwareSystemId, startDate);
        decimal additionalDiscount = new decimal(0);
        if (await IsPreviousClientAsync(contract.clientId, token))
        {
            additionalDiscount = new decimal(5);
        }

        decimal priceAfterDiscounts =
            CalculateTheFinalPrice(basePrice, discount, additionalDiscount, contract.additionalYearsOfSupport);

        await AddNewContractAsync(priceAfterDiscounts, startDate, endDate, contract, token);

        return "Successfully added new contract!";
    }

    public async Task<bool> IsContractInDbAsync(PaymentDTO pay, CancellationToken token)
    {
        return await _contractRepository.IsContractInDbAsync(pay, token);
    }

    public bool IsPaymentCorrect(decimal pay)
    {
        return _contractRepository.IsPaymentCorrect(pay);
    }

    public async Task<Contract> GetContractAsync(int contractId, CancellationToken token)
    {
        return await _contractRepository.GetContractAsync(contractId, token);
    }

    public async Task<int> AddPaymentToContractAsync(PaymentDTO pay, Contract contract, CancellationToken token)
    {
        return await _contractRepository.AddPaymentToContractAsync(pay, contract, token);
    }

    public async Task<int> AddAndSignInTransitionAsync(PaymentDTO pay, Contract contract, CancellationToken token)
    {
        return await _contractRepository.AddAndSignInTransitionAsync(pay, contract, token);
    }

    public async Task<string> PayForContract(PaymentDTO pay, CancellationToken token)
    {
        if (!await IsContractInDbAsync(pay, token))
        {
            throw new Exception($"Error: Cannot find contract with id {pay.contractId}.");
        }

        if (!IsPaymentCorrect(pay.payment))
        {
            throw new Exception("Error: Payment cannot be lower than or equal to 0.");
        }

        Contract contract = await GetContractAsync(pay.contractId, token);

        if (contract.CurrentCharge + pay.payment == contract.MaxCharge)
        {
            int transDec = await AddAndSignInTransitionAsync(pay, contract, token);
            if (transDec == -1)
            {
                throw new Exception("Error: Transaction rollbacked.");
            }

            return "Contract properly paid completely.";
        }
        else
        {
            await AddPaymentToContractAsync(pay, contract, token);
            return "Part of the contract paid correctly.";
        }

    }
}