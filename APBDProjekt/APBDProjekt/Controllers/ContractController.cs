using APBDProjekt.DTOs.Contract;
using APBDProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBDProjekt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContractController: ControllerBase
{
    private readonly IContractService _contactService;

    public ContractController(IContractService contactService)
    {
        _contactService = contactService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddNewContractWithSoftwareAndClientAsync(ContractDTO contract,
        CancellationToken token)
    {
        string message = await _contactService.CreateNewContractAsync(contract, token);

        return Ok(message);
    }
    
    [Authorize]
    [HttpPut("{contractId}")]
    public async Task<IActionResult> PutPaymentForContractAsync(int contractId, decimal payment, CancellationToken token)
    {
        PaymentDTO paymentDto = new PaymentDTO() { contractId = contractId, payment = payment };

        string message = await _contactService.PayForContract(paymentDto, token);

        return Ok(message);
    }
}