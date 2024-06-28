using APBDProjekt.DTOs.Client;
using APBDProjekt.DTOs.Contract;
using APBDProjekt.Entities;
using Microsoft.EntityFrameworkCore;

namespace APBDProjekt.Repositories;

public class ContractRepository: IContractRepository
{
    private readonly SoftwareDistributionDbContext _distributionDbContext;

    public ContractRepository(SoftwareDistributionDbContext distributionDbContext)
    {
        _distributionDbContext = distributionDbContext;
    }

    public async Task<bool> IsSoftwareSystemInDbAsync(int id, CancellationToken token)
    {
        return await _distributionDbContext.SoftwareSystems.AnyAsync(s => s.SoftwareSystemId == id, token);
    }

    public async Task<bool> IsClientInDbAsync(int id, CancellationToken token)
    {
        return await _distributionDbContext.Clients.AnyAsync(c => c.ClientId == id && c.IsDeleted == false, token);
    }

    public async Task<bool> DoesClientHaveContractOnThatSoft(int clientId, int softwareSystemId, CancellationToken token)
    {
        return await _distributionDbContext.Contracts.AnyAsync(c =>
            c.ClientId == clientId && c.SoftwareSystemId == softwareSystemId && c.EndDate.AddYears(c.YearsOfSupport) >= DateTime.Today && c.IsSigned == true);
    }

    public bool IsYearsOfSupportCorrect(int years)
    {
        return years is 0 or 1 or 2 or 3;
    }

    public bool IsDaysForSigningCorrect(int days)
    {
        return days is >= 3 and <= 30;
    }

    public DateTime GetEndDate(DateTime startDate, int days)
    {
        return startDate.AddDays(days);
    }

    public async Task<decimal> GetBasePriceAsync(int soft, CancellationToken token)
    {
        return await _distributionDbContext.SoftwareSystems.Where(s => s.SoftwareSystemId == soft).Select(s => s.Price).FirstAsync(token);
    }

    public bool IsInTimeslot(DateTime date, string timeslot)
    {
        string[] months = timeslot.Split(" ");
        string[] startMonth = months[0].Split('-');
        string[] endMonth = months[1].Split('-');

        DateTime startDate = new DateTime(date.Year, int.Parse(startMonth[1]), int.Parse(startMonth[0]));
        DateTime endDate = new DateTime(date.Year, int.Parse(endMonth[1]), int.Parse(endMonth[0]));
        
        if (endDate < startDate) endDate = endDate.AddYears(1); 

        return date >= startDate && date <= endDate;
    }

    public decimal GetMaxDiscount(int soft, DateTime date)
    {
        return _distributionDbContext.Discounts.Include(d => d.IdSoftwareSystems)
            .Where(d => d.IdSoftwareSystems.Any(s => s.SoftwareSystemId == soft)).ToList()
            .Where(f => IsInTimeslot(date, f.Timeslot)).Max(d => d.Value);
    }

    public async Task<bool> IsPreviousClientAsync(int id, CancellationToken token)
    {
        return await _distributionDbContext.Contracts.AnyAsync(c => c.ClientId == id && c.IsSigned == true, token);
    }

    public decimal CalculateTheFinalPrice(decimal basePrice, decimal discount, decimal additionalDiscount, int yearsOfSupport)
    {
        return basePrice - (discount + additionalDiscount) / 100 * basePrice + (yearsOfSupport) * 1000;
    }

    public async Task<int> AddNewContractAsync(decimal finalPrice, DateTime startDate, DateTime endDate, ContractDTO contract, CancellationToken token)
    {
        await _distributionDbContext.Contracts.AddAsync(new Contract()
        {
            CurrentCharge = new decimal(0),
            MaxCharge = finalPrice,
            StartDate = startDate,
            EndDate = endDate,
            IsSigned = false,
            YearsOfSupport = contract.additionalYearsOfSupport + 1,
            SoftwareSystemId = contract.softwareSystemId,
            ClientId = contract.clientId

        }, token);
        await _distributionDbContext.SaveChangesAsync(token);
        return 0;
    }

    public async Task<bool> IsContractInDbAsync(PaymentDTO pay, CancellationToken token)
    {
        return await _distributionDbContext.Contracts.AnyAsync(s => s.ContractId == pay.contractId, token);
    }

    public bool IsPaymentCorrect(decimal pay)
    {
        return pay > 0;
    }

    public async Task<Contract> GetContractAsync(int contractId, CancellationToken token)
    {
        return await _distributionDbContext.Contracts.SingleAsync(c => c.ContractId == contractId, token);
    }

    public async Task<int> AddPaymentToContractAsync(PaymentDTO pay, Contract contract, CancellationToken token)
    {
        if (contract.CurrentCharge + pay.payment > contract.MaxCharge)
        {
            throw new Exception("Error: Cannot pay more than contract requires.");
        }

        contract.CurrentCharge += pay.payment;
        await _distributionDbContext.SaveChangesAsync(token);
        return 0;
    }

    public async Task<int> ContractSignedAsync(Contract contract, CancellationToken token)
    {
        contract.IsSigned = true;
        await _distributionDbContext.SaveChangesAsync(token);
        return 0;
    }

    public async Task<int> AddAndSignInTransitionAsync(PaymentDTO pay, Contract contract, CancellationToken token)
    {
        using var transaction = await _distributionDbContext.Database.BeginTransactionAsync(token);
        try
        {
            await AddPaymentToContractAsync(pay, contract, token);
            await ContractSignedAsync(contract, token);
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