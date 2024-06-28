namespace APBDProjekt.Services;

public interface IRevenueService
{
    public Task<decimal> GetCurrencyConversionAsync(string targetCur, CancellationToken token);

    public Task<bool> IsSoftwareSystemInDbAsync(int? id, CancellationToken token);
    public Task<decimal> GetRevenueForAllProductsAsync(CancellationToken token);
    public Task<decimal> GetRevenueForOneProductAsync(int? softwareSystemId, CancellationToken token);

    public Task<decimal> GetRevenue(int? softwareSystemId, CancellationToken token, string? currencyName = null);
    
    public Task<decimal> GetAnticipatedRevenueForAllProductsAsync(CancellationToken token, string? currencyName = null);
    public Task<decimal> GetAnticipatedRevenueForOneProductsAsync(int? softwareSystemId, CancellationToken token, string? currencyName = null);
    
    public Task<decimal> GetAnticipatedRevenue(int? softwareSystemId, CancellationToken token, string? currencyName = null);
}