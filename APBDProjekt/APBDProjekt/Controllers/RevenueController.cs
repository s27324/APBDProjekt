using APBDProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBDProjekt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RevenueController: ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    [Authorize]
    [HttpGet("contract/revenue")]
    public async Task<IActionResult> GetRevenueFromAllProductsAsync(CancellationToken token, string? currencyName = null)
    {
        return Ok(await _revenueService.GetRevenue(null, token, currencyName));
    }
    [Authorize]
    [HttpGet("contract/revenue/{softwareSystemId}")]
    public async Task<IActionResult> GetRevenueFromAllProductsAsync(int softwareSystemId, CancellationToken token,
        string? currencyName = null)
    {
        return Ok(await _revenueService.GetRevenue(softwareSystemId, token, currencyName));
    }
    [Authorize]
    [HttpGet("contract/anticipatedRevenue")]
    public async Task<IActionResult> GetAnticipatedRevenueFromAllProductsAsync(CancellationToken token, string? currencyName = null)
    {
        return Ok(await _revenueService.GetAnticipatedRevenue(null, token, currencyName));
    }
    [Authorize]
    [HttpGet("contract/anticipatedRevenue/{softwareSystemId}")]
    public async Task<IActionResult> GetAnticipatedRevenueFromAllProductsAsync(int softwareSystemId, CancellationToken token, string? currencyName = null)
    {
        return Ok(await _revenueService.GetAnticipatedRevenue(softwareSystemId, token, currencyName));
    }
}