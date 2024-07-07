using APBDProjekt.Repositories;
using APBDProjekt.Services;
using Moq;

namespace APBDProjektTest.UnitTests;

public class ContractServiceTests
{
    private readonly Mock<IContractRepository> _mockContractRepository;
    private readonly IContractService _contractService;

    public ContractServiceTests()
    {
        _mockContractRepository = new Mock<IContractRepository>();
        _contractService = new ContractService(_mockContractRepository.Object);
    }
    
    [Fact]
    public async Task IsSoftwareSystemInDbAsync_ReturnsFalse_WhenSoftwareSystemIsNotInDb()
    {
        int id = 0;
        CancellationToken cancellationToken = new CancellationToken();
        _mockContractRepository.Setup(repo => repo.IsSoftwareSystemInDbAsync(id, cancellationToken)).ReturnsAsync(false);

        var result = await _contractService.IsSoftwareSystemInDbAsync(id, cancellationToken);
        
        Assert.False(result);
    }
}