using APBDProjekt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APBDProjekt.Services;

public class RevenueService : IRevenueService
{
    private readonly IRevenueRepository _revenueRepository;

    public RevenueService(IRevenueRepository revenueRepository)
    {
        _revenueRepository = revenueRepository;
    }

    public async Task<decimal> GetCurrencyConversionAsync(string targetCur, CancellationToken token)
    {
        return await _revenueRepository.GetCurrencyConversionAsync(targetCur, token);
    }

    public async Task<bool> IsSoftwareSystemInDbAsync(int? id, CancellationToken token)
    {
        return await _revenueRepository.IsSoftwareSystemInDbAsync(id, token);
    }

    public async Task<decimal> GetRevenueForAllProductsAsync(CancellationToken token)
    {
        return await _revenueRepository.GetRevenueForAllProductsAsync(token);
    }

    public async Task<decimal> GetRevenueForOneProductAsync(int? softwareSystemId, CancellationToken token)
    {
        return await _revenueRepository.GetRevenueForOneProductAsync(softwareSystemId, token);
    }

    public async Task<decimal> GetRevenue(int? softwareSystemId, CancellationToken token, string? currencyName = null)
    {
        if (softwareSystemId is null)
        {
            if (currencyName is null)
            {
                return await GetRevenueForAllProductsAsync(token);
            }
            return Math.Round(await GetRevenueForAllProductsAsync(token)/await GetCurrencyConversionAsync(currencyName, token),2);
        }

        if (!await IsSoftwareSystemInDbAsync(softwareSystemId, token))
        {
            throw new Exception($"Error: Cannot find software system with id {softwareSystemId}.");
        }
        if (currencyName is null)
        {
            return await GetRevenueForOneProductAsync(softwareSystemId, token);
        }
        return Math.Round(await GetRevenueForOneProductAsync(softwareSystemId, token)/await GetCurrencyConversionAsync(currencyName, token), 2);
        
    }

    public async Task<decimal> GetAnticipatedRevenueForAllProductsAsync(CancellationToken token, string? currencyName = null)
    {
        return await _revenueRepository.GetAnticipatedRevenueForAllProductsAsync(token);
    }

    public async Task<decimal> GetAnticipatedRevenueForOneProductsAsync(int? softwareSystemId, CancellationToken token, string? currencyName = null)
    {
        return await _revenueRepository.GetAnticipatedRevenueForOneProductsAsync(softwareSystemId, token);
    }

    public async Task<decimal> GetAnticipatedRevenue(int? softwareSystemId, CancellationToken token, string? currencyName = null)
    {
        if (softwareSystemId is null)
        {
            if (currencyName is null)
            {
                return await GetAnticipatedRevenueForAllProductsAsync(token);
            }
            return Math.Round(await GetAnticipatedRevenueForAllProductsAsync(token)/await GetCurrencyConversionAsync(currencyName, token),2);
            
        }

        if (!await IsSoftwareSystemInDbAsync(softwareSystemId, token))
        {
            throw new Exception($"Error: Cannot find software system with id {softwareSystemId}.");
        }
        if (currencyName is null)
        {
            return await GetAnticipatedRevenueForOneProductsAsync(softwareSystemId, token);
        }
        return Math.Round(await GetAnticipatedRevenueForOneProductsAsync(softwareSystemId, token)/await GetCurrencyConversionAsync(currencyName, token), 2);
    }
}