using APBDProjekt.DTOs.Contract;
using APBDProjekt.Entities;

namespace APBDProjekt.Repositories;

public interface IContractRepository
{
    public Task<bool> IsSoftwareSystemInDbAsync(int id, CancellationToken token);
    public Task<bool> IsClientInDbAsync(int id, CancellationToken token);
    public Task<bool> DoesClientHaveContractOnThatSoft(int clientId, int softwareSystemId, CancellationToken token);
    public bool IsYearsOfSupportCorrect(int years);
    public bool IsDaysForSigningCorrect(int days);
    public DateTime GetEndDate(DateTime startDate, int days);
    public Task<decimal> GetBasePriceAsync(int soft, CancellationToken token);
    public bool IsInTimeslot(DateTime date, string timeslot);
    public decimal GetMaxDiscount(int soft, DateTime date);
    public Task<bool> IsPreviousClientAsync(int id, CancellationToken token);
    public decimal CalculateTheFinalPrice(decimal basePrice, decimal discount, decimal additionalDiscount,
        int yearsOfSupport);
    public Task<int> AddNewContractAsync(decimal finalPrice, DateTime startDate, DateTime endDate, ContractDTO contract, CancellationToken token);
    
    
    public Task<bool> IsContractInDbAsync(PaymentDTO pay, CancellationToken token);
    public bool IsPaymentCorrect(decimal pay);
    public Task<Contract> GetContractAsync(int contractId, CancellationToken token);
    public Task<int> AddPaymentToContractAsync(PaymentDTO pay, Contract contract, CancellationToken token);
    public Task<int> ContractSignedAsync(Contract contract, CancellationToken token);
    public Task<int> AddAndSignInTransitionAsync(PaymentDTO pay, Contract contract, CancellationToken token);
}