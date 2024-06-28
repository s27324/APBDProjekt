using System.Globalization;
using APBDProjekt.Entities;
using Microsoft.EntityFrameworkCore;

namespace APBDProjekt.Repositories;

public class RevenueRepository: IRevenueRepository
{
    private readonly SoftwareDistributionDbContext _distributionDbContext;
    private const string apiKey = "399a22f0a2bd472f96303b6875c4717d";
    private const string url = $"https://openexchangerates.org/api/latest.json?app_id={apiKey}";


    public RevenueRepository(SoftwareDistributionDbContext distributionDbContext)
    {
        _distributionDbContext = distributionDbContext;
    }


    public async Task<decimal> GetCurrencyConversionAsync(string targetCur, CancellationToken token)
    {
        if (targetCur != "PLN")
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url, token);

            string response = await responseMessage.Content.ReadAsStringAsync(token);

            int startPln = response.IndexOf("\"PLN\":") + "\"PLN\":".Length;
            int endPln = response.IndexOf(',', startPln);

            if (endPln == -1)
            {
                endPln = response.IndexOf('}', startPln);
            }

            string exchangePln = response.Substring(startPln, endPln - startPln).Trim();

            decimal plnRate = decimal.Parse(exchangePln, CultureInfo.InvariantCulture);

            if (targetCur != "USD")
            {
                string targetSearch = $"\"{targetCur}\":";
                int startTarget = response.IndexOf(targetSearch) + targetSearch.Length;
                int endTarget = response.IndexOf(',', startTarget);
                Console.WriteLine(endTarget);
                if (endTarget == -1)
                {
                    endTarget = response.IndexOf('}', startTarget);
                }
                Console.WriteLine(endTarget);
                if (endTarget == 79)
                {
                    throw new Exception($"Error: Cannot find currency with code: {targetCur}.");
                }

                string exchangeTarget = response.Substring(startTarget, endTarget - startTarget).Trim();

                decimal targetRate = decimal.Parse(exchangeTarget, CultureInfo.InvariantCulture);

                return plnRate / targetRate;
            }

            return plnRate;
        }

        return new decimal(1);
    }

    public async Task<bool> IsSoftwareSystemInDbAsync(int? id, CancellationToken token)
    {
        return await _distributionDbContext.SoftwareSystems.AnyAsync(s => s.SoftwareSystemId == id, token);
    }

    public async Task<decimal> GetRevenueForAllProductsAsync(CancellationToken token)
    {
        return await _distributionDbContext.Contracts.Where(c => c.IsSigned == true).SumAsync(c => c.MaxCharge, token);
    }

    public async Task<decimal> GetRevenueForOneProductAsync(int? softwareSystemId, CancellationToken token)
    {
        return await _distributionDbContext.Contracts.Where(c => c.IsSigned == true && c.SoftwareSystemId == softwareSystemId).SumAsync(c => c.MaxCharge, token);

    }

    public async Task<decimal> GetAnticipatedRevenueForAllProductsAsync(CancellationToken token)
    {
        return await _distributionDbContext.Contracts.Where(c => c.IsSigned == true || (c.IsSigned == false && c.EndDate >= DateTime.Today)).SumAsync(c => c.MaxCharge, token);
    }

    public async Task<decimal> GetAnticipatedRevenueForOneProductsAsync(int? softwareSystemId, CancellationToken token)
    {
        return await _distributionDbContext.Contracts.Where(c => (c.IsSigned == true || (c.IsSigned == false && c.EndDate >= DateTime.Today)) && c.SoftwareSystemId == softwareSystemId).SumAsync(c => c.MaxCharge, token);
    }
}