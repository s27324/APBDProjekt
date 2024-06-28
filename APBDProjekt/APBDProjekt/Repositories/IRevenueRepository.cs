namespace APBDProjekt.Repositories;

public interface IRevenueRepository
{
    public Task<decimal> GetCurrencyConversionAsync(string targetCur, CancellationToken token);
    public Task<bool> IsSoftwareSystemInDbAsync(int? id, CancellationToken token);

    public Task<decimal> GetRevenueForAllProductsAsync(CancellationToken token);
    public Task<decimal> GetRevenueForOneProductAsync(int? softwareSystemId, CancellationToken token);

    public Task<decimal> GetAnticipatedRevenueForAllProductsAsync(CancellationToken token);
    public Task<decimal> GetAnticipatedRevenueForOneProductsAsync(int? softwareSystemId, CancellationToken token);
    
}