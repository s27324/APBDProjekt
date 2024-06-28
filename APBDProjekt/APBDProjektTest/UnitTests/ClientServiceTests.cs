using APBDProjekt.Repositories;
using APBDProjekt.Services;
using Moq;

namespace APBDProjektTest.UnitTests;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _clientService = new ClientService(_mockClientRepository.Object);
    }

    [Fact]
    public void IsPESELTheRightSize_ReturnsTrue_WhenPESELIs11Digits()
    {
        string PESEL = "12345678910";
        _mockClientRepository.Setup(repo => repo.IsPESELTheRightSize(PESEL)).Returns(true);

        var result = _clientService.IsPESELTheRightSize(PESEL);
        
        Assert.True(result);
    }
    
    [Fact]
    public void IsPESELOnlyDigits_ReturnsFalse_WhenPESELContainsLetters()
    {
        string pesel = "12345abcd01";
        _mockClientRepository.Setup(repo => repo.IsPESELOnlyDigits(pesel)).Returns(false);

        var result = _clientService.IsPESELOnlyDigits(pesel);

        Assert.False(result);
    }

    [Fact]
    public void DoPhoneNumberHaveAreaCode_ReturnsNegativeOne_WhenPhoneNumberIsNull()
    {
        string phoneNumber = null;
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(phoneNumber)).Returns(-1);

        var result = _clientService.DoPhoneNumberHaveAreaCode(phoneNumber);

        Assert.Equal(-1, result);
    }
}