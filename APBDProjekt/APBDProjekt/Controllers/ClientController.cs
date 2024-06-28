using APBDProjekt.DTOs.Client;
using APBDProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBDProjekt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController: ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [Authorize]
    [HttpPost("privateClient")]
    public async Task<IActionResult> PostPrivateClientAsync(PrivateClientDTO client, CancellationToken token)
    {
        string message = await _clientService.AddNewPrivateClientAndClientAsync(client, token);

        return Ok(message);
    }
    
    [Authorize]
    [HttpPost("companyClient")]
    public async Task<IActionResult> PostCompanyClientAsync(CompanyClientDTO client, CancellationToken token)
    {
        string message = await _clientService.AddNewCompanyAndClientAsync(client, token);

        return Ok(message);
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("privateClient/{clientId}/delete")]
    public async Task<IActionResult> DeletePrivateClientAsync(int clientId, CancellationToken token)
    {
        string message = await _clientService.DeletePrivateClientAsync(clientId, token);

        return Ok(message);
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("privateClient/{clientId}/update")]
    public async Task<IActionResult> UpdatePrivateClientInformationAsync(int clientId, UpdatePrivateClientDTO client, CancellationToken token)
    {
        string message = await _clientService.UpdatePrivateClientInfoAsync(clientId, client, token);

        return Ok(message);
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("companyClient/{clientId}/update")]
    public async Task<IActionResult> UpdateCompanyClientInformationAsync(int clientId, UpdateCompanyClientDTO client, CancellationToken token)
    {
        string message = await _clientService.UpdateCompanyClientInfoAsync(clientId, client, token);

        return Ok(message);
    }
}