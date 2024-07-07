using APBDProjekt.DTOs.Client;
using APBDProjekt.Entities;
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
    public void DoPhoneNumberHaveAreaCode_ReturnMinusOne_WhenPhoneNumberIsNull()
    {
        string phoneNumber = null;
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(phoneNumber)).Returns(-1);

        var result = _clientService.DoPhoneNumberHaveAreaCode(phoneNumber);

        Assert.Equal(-1, result);
    }
    [Fact]
    public void DoPhoneNumberHaveAreaCode_ReturnMinusTwo_WhenPhoneNumberIsNotMatchingPattern()
    {
        string phoneNumber = "+4777777777";
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(phoneNumber)).Returns(-2);

        var result = _clientService.DoPhoneNumberHaveAreaCode(phoneNumber);

        Assert.Equal(-2, result);
    }

    [Fact]
    public void EmailAddressValid_ReturnMinusTwo_WhenEmailAddresIsNull()
    {
        string email = null;
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(email)).Returns(-2);

        var result = _clientService.EmailAddressValid(email);
        
        Assert.Equal(-2, result);
    }
    
    [Fact]
    public void EmailAddressValid_ReturnMinusOne_WhenEmailAddresIsNotEmail()
    {
        string email = "kacper";
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(email)).Returns(-1);

        var result = _clientService.EmailAddressValid(email);
        
        Assert.Equal(-1, result);
    }
    
    [Fact]
    public void IsOnlyLetters_ReturnFalse_WhenWordConsistsOfNotOnlyWords()
    {
        string word = "Kacper @Alot";
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(word)).Returns(false);

        var result = _clientService.IsOnlyLetters(word);
        
        Assert.False(result);
    }
    
    [Fact]
    public void IsEmptyOrWhiteSpace_ReturnFalse_WhenIsWhiteSpace()
    {
        string word = " ";
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(word)).Returns(false);
        
        var result = _clientService.IsEmptyOrWhiteSpace(word);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task PrivateClientExistsAsync_ReturnTrue_WhenPrivateClientExists()
    {
        string PESEL = "12345678910";
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.PrivateClientExistsAsync(PESEL, token)).ReturnsAsync(true);

        var result = await _clientService.PrivateClientExistsAsync(PESEL, token);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task PrivateClientExistsAsync_ReturnFalse_WhenPrivateClientDoesntExist()
    {
        string PESEL = "12345678910";
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.PrivateClientExistsAsync(PESEL, token)).ReturnsAsync(false);

        var result = await _clientService.PrivateClientExistsAsync(PESEL, token);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenPhoneNumberIsNull()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = "Alot", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = null, email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(-1);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Phone number cannot be null",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenPhoneNumberDoesntMatchThePattern()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = "Alot", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+ 565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(-2);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenEmailIsInvalid()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = "Alot", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);

        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(-1);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Email is invalid. Please remember about @somemail.someending.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenEmailIsEmpty()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = "Alot", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = ""
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(-2);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Email address cannot be empty.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ReturnSuccess_WhenAllIsGood()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = "Alot", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsPESELOnlyDigits(client.PESEL)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsPESELTheRightSize(client.PESEL)).Returns(true);
        _mockClientRepository.Setup(repo => repo.PrivateClientExistsAsync(client.PESEL, token)).ReturnsAsync(false);
        _mockClientRepository.Setup(repo => repo.AddingInTransactionPrivateAsync(client, token)).ReturnsAsync(1);

        var result = await _clientService.AddNewPrivateClientAndClientAsync(client, token);
        Assert.Equal("Succesffully added new private client with id 1.", result);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenFirstNameIsEmpty()
    {
        var client = new PrivateClientDTO()
        {
            firstName = " ", lastName = "Alot", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(true);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Client's firstname cannot be empty or whitespace",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenFirstNameIsNotOnlyLetters()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper#1", lastName = "Alot", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(false);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Client's firstname should be only made from letters.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenLastNameIsEmpty()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = " ", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(true);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Client's lastname cannot be empty or whitespace",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenLastNameIsNotOnlyLetters()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = "Alot%1", PESEL = "03030303033", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.lastName)).Returns(false);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Client's lastname should be only made from letters.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenAddressIsEmptyOrWhiteSpace()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = "Alot", PESEL = "03030303033", address = " ",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(true);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Client's address cannot be empty or whitespace.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenPESELIsNotOnlyDigits()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = " ", PESEL = "0303030303d", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsPESELOnlyDigits(client.PESEL)).Returns(false);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: PESEL should only consist of digits.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenPESELIsNotTheRightSize()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = " ", PESEL = "030303030311", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsPESELOnlyDigits(client.PESEL)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsPESELTheRightSize(client.PESEL)).Returns(false);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: PESEL should be exactly 11 digits long.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenPrivateClientExists()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = " ", PESEL = "030303030311", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsPESELOnlyDigits(client.PESEL)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsPESELTheRightSize(client.PESEL)).Returns(true);
        _mockClientRepository.Setup(repo => repo.PrivateClientExistsAsync(client.PESEL, token)).ReturnsAsync(true);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal($"Error: Private client with PESEL: 030303030311 already exists.",exc.Message);
    }
    [Fact]
    public async Task AddNewPrivateClientAndClientAsync_ThrowException_WhenTransactionRollback()
    {
        var client = new PrivateClientDTO()
        {
            firstName = "Kacper", lastName = " ", PESEL = "030303030311", address = "adres adres 05",
            phoneNumber = "+48 565565565", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.firstName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.lastName)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(client.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsPESELOnlyDigits(client.PESEL)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsPESELTheRightSize(client.PESEL)).Returns(true);
        _mockClientRepository.Setup(repo => repo.PrivateClientExistsAsync(client.PESEL, token)).ReturnsAsync(false);
        _mockClientRepository.Setup(repo => repo.AddingInTransactionPrivateAsync(client, token)).ReturnsAsync(-1);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewPrivateClientAndClientAsync(client, token));
        Assert.Equal("Error: Transaction rollbacked.",exc.Message);
    }
    
    [Fact]
    public void IsKRSTheRightSize_ReturnFalse_WhenKRSLengthDoesntEqualNineOrForteen()
    {
        string KRS = "55555";
        _mockClientRepository.Setup(repo => repo.IsKRSTheRightSize(KRS)).Returns(false);
        
        var result = _clientService.IsKRSTheRightSize(KRS);
        
        Assert.False(result);
    }
    
    [Fact]
    public void IsKRSOnlyDigits_ReturnFalse_WhenKRSIsNotOnlyDigits()
    {
        string KRS = "55555555d";
        _mockClientRepository.Setup(repo => repo.IsKRSOnlyDigits(KRS)).Returns(false);
        
        var result = _clientService.IsKRSOnlyDigits(KRS);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task CompanyClientExistsAsync_ReturnFalse_WhenCompanyClientDoesntExist()
    {
        string KRS = "12345678910";
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.CompanyClientExistsAsync(KRS, token)).ReturnsAsync(false);

        var result = await _clientService.CompanyClientExistsAsync(KRS, token);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task AddingInTransactionCompanyAsync_ReturnTrue_WhenCompanyClientAddedSuccessfully()
    {
        var company = new CompanyClientDTO()
        {
            KRS = "999999999", address = "adres adres 05", phoneNumber = "+46 123123123", email = "kacper@gmail.com",
            name = "nazwa"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.AddingInTransactionCompanyAsync(company, token)).ReturnsAsync(1);

        var result = await _clientService.AddingInTransactionCompanyAsync(company, token);
        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenPhoneNumberIsNull()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = null, email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(-1);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: Phone number cannot be null",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenPhoneNumberDoesntMatchThePattern()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = "+454 5", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(-2);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenEmailIsInvalid()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);

        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(-1);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: Email is invalid. Please remember about @somemail.someending",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenEmailIsEmpty()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = ""
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(-2);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: Email address cannot be empty.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ReturnSuccess_WhenAllIsGood()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.name)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsKRSOnlyDigits(client.KRS)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsKRSTheRightSize(client.KRS)).Returns(true);
        _mockClientRepository.Setup(repo => repo.CompanyClientExistsAsync(client.KRS, token)).ReturnsAsync(false);
        _mockClientRepository.Setup(repo => repo.AddingInTransactionCompanyAsync(client, token)).ReturnsAsync(1);

        var result = await _clientService.AddNewCompanyAndClientAsync(client, token);
        Assert.Equal("Succesffully added new company client with id 1.", result);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenNameIsEmpty()
    {
        var client = new CompanyClientDTO()
        {
            name = " ",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.name)).Returns(true);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: Company's name cannot be empty or whitespace",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenAddressIsEmptyOrWhiteSpace()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = " ",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.name)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(true);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: Client's address cannot be empty or whitespace.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenKRSIsNotOnlyDigits()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030d", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.name)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsKRSOnlyDigits(client.KRS)).Returns(false);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: KRS should only consist of digits.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenKRSIsNotTheRightSize()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "0303030303", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.name)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsKRSOnlyDigits(client.KRS)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsKRSTheRightSize(client.KRS)).Returns(false);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: KRS should be exactly 9 or 14 digits long.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenCompanyAlreadyExists()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.name)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsKRSOnlyDigits(client.KRS)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsKRSTheRightSize(client.KRS)).Returns(true);
        _mockClientRepository.Setup(repo => repo.CompanyClientExistsAsync(client.KRS, token)).ReturnsAsync(true);

        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal($"Error: Company client with KRS: 030303030 already exists.",exc.Message);
    }
    
    [Fact]
    public async Task AddNewCompanyAndClientAsync_ThrowException_WhenTransactionRollbacked()
    {
        var client = new CompanyClientDTO()
        {
            name = "Kacper",  KRS = "030303030", address = "adres adres 05",
            phoneNumber = "+454 523523523", email = "kacper@gmail.com"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(client.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(client.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.name)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsEmptyOrWhiteSpace(client.address)).Returns(false);
        _mockClientRepository.Setup(repo => repo.IsKRSOnlyDigits(client.KRS)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsKRSTheRightSize(client.KRS)).Returns(true);
        _mockClientRepository.Setup(repo => repo.CompanyClientExistsAsync(client.KRS, token)).ReturnsAsync(false);
        _mockClientRepository.Setup(repo => repo.AddingInTransactionCompanyAsync(client, token)).ReturnsAsync(-1);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.AddNewCompanyAndClientAsync(client, token));
        Assert.Equal("Error: Transaction rollbacked.",exc.Message);
    }
    
    [Fact]
    public async Task IsPrivateClientInDbAsync_ReturnFalse_WhenCannotFindClientInDb()
    {
        int id = 0;
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(false);
        
        var result = await _clientService.IsPrivateClientInDbAsync(id, token);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task GetClientAsync_ReturnClient_WhenFoundClient()
    {
        int id = 1;
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = 1,
            CompanyId = null
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.GetClientAsync(id, token)).ReturnsAsync(client);
        
        var result = await _clientService.GetClientAsync(id, token);
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task SoftDeletePrivateClientAsync_ReturnZero_WhenClientIsSoftDeleted()
    {
        int id = 1;
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = 1,
            CompanyId = null
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.SoftDeletePrivateClientAsync(client, token)).ReturnsAsync(0);
        
        var result = await _clientService.SoftDeletePrivateClientAsync(client, token);
        
        Assert.Equal(0, result);
    }
    
    [Fact]
    public async Task DeletePrivateClientAsync_ThrowException_WhenCannotFindPrivateClient()
    {
        int id = 0;
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(false);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.DeletePrivateClientAsync(id, token));
        Assert.Equal("Error: Private client with id 0 not found or had been previously deleted.",exc.Message);
    }
    
    [Fact]
    public async Task DeletePrivateClientAsync_SoftDeleting_WhenPrivateClientIsFound()
    {
        int id = 1;
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = 1,
            CompanyId = null
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.SoftDeletePrivateClientAsync(client, token)).ReturnsAsync(0);
        
        var result = await _clientService.DeletePrivateClientAsync(id, token);
        Assert.Equal("Successfully deleted private client with id 1.", result);
    }
    
    [Fact]
    public async Task UpdatePrivateClientAsync_ReturnMinusOne_WhenTransactionRollbacked()
    {
        int id = 1;
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = 1,
            CompanyId = null
        };
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper",
            lastName = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.UpdatePrivateClientAsync(updateClient, client, token)).ReturnsAsync(-1);
        
        var result = await _clientService.UpdatePrivateClientAsync(updateClient, client, token);
        Assert.Equal(-1, result);
    }
    
    [Fact]
    public async Task UpdatePrivateClientInfoAsync_ThrowException_WhenPrivateClientisNotInDb()
    {
        int id = 0;
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper",
            lastName = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(false);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdatePrivateClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Private client with id 0 not found or had been previously deleted.",exc.Message);
    }
    
    [Fact]
    public async Task UpdatePrivateClientInfoAsync_ThrowException_WhenFirstNameIsNotOnlyLetters()
    {
        int id = 1;
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper%$1",
            lastName = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.firstName)).Returns(false);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdatePrivateClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Client's firstname should be only made from letters.",exc.Message);
    }
    
    [Fact]
    public async Task UpdatePrivateClientInfoAsync_ThrowException_WhenLastNameIsNotOnlyLetters()
    {
        int id = 1;
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper",
            lastName = "Alot1$",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.lastName)).Returns(false);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdatePrivateClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Client's lastname should be only made from letters.",exc.Message);
    }
    
    [Fact]
    public async Task UpdatePrivateClientInfoAsync_ThrowException_WhenPhoneNumberDoesntFitThePattern()
    {
        int id = 1;
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper",
            lastName = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(updateClient.phoneNumber)).Returns(-2);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdatePrivateClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits",exc.Message);
    }
    
    [Fact]
    public async Task UpdatePrivateClientInfoAsync_ThrowException_WhenEmailIsInvalid()
    {
        int id = 1;
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper",
            lastName = "Alot",
            address = "address 04",
            email = "alot",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(updateClient.email)).Returns(-1);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdatePrivateClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Email is invalid. Please remember about @somemail.someending",exc.Message);
    }
    [Fact]
    public async Task UpdatePrivateClientInfoAsync_ThrowException_WhenTransactionRolbacked()
    {
        int id = 1;
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper",
            lastName = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = 1,
            CompanyId = null
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(updateClient.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(updateClient.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.GetClientAsync(id, token)).ReturnsAsync(client);
        _mockClientRepository.Setup(repo => repo.UpdatePrivateClientAsync(updateClient, client, token)).ReturnsAsync(-1);

        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdatePrivateClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Transaction rollbacked.",exc.Message);
    }
    
    [Fact]
    public async Task UpdatePrivateClientInfoAsync_ReturnSuccess_WhenPrivateClientisUpdated()
    {
        int id = 1;
        UpdatePrivateClientDTO updateClient = new UpdatePrivateClientDTO()
        {
            firstName = "Kacper",
            lastName = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = 1,
            CompanyId = null
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsPrivateClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.firstName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.IsOnlyLetters(updateClient.lastName)).Returns(true);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(updateClient.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(updateClient.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.GetClientAsync(id, token)).ReturnsAsync(client);
        _mockClientRepository.Setup(repo => repo.UpdatePrivateClientAsync(updateClient, client, token)).ReturnsAsync(0);

        
        var result = await _clientService.UpdatePrivateClientInfoAsync(id, updateClient, token);
        Assert.Equal("Successfully updated private client with id 1.", result);
    }
    
    [Fact]
    public async Task IsCompanyClientInDbAsync_ReturnFalse_WhenCannotFindClientInDb()
    {
        int id = 0;
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsCompanyClientInDbAsync(id, token)).ReturnsAsync(false);
        
        var result = await _clientService.IsCompanyClientInDbAsync(id, token);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task UpdateCompanyClientAsync_ReturnMinusOne_WhenTransactionRollbacked()
    {
        int id = 1;
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = null,
            CompanyId = 1
        };
        UpdateCompanyClientDTO updateClient = new UpdateCompanyClientDTO()
        {
            name = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.UpdateCompanyClientAsync(updateClient, client, token)).ReturnsAsync(-1);
        
        var result = await _clientService.UpdateCompanyClientAsync(updateClient, client, token);
        Assert.Equal(-1, result);
    }
    
    [Fact]
    public async Task UpdateCompanyClientInfoAsync_ThrowException_WhenClientIsNotInDB()
    {
        int id = 0;
        UpdateCompanyClientDTO updateClient = new UpdateCompanyClientDTO()
        {
            name = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsCompanyClientInDbAsync(id, token)).ReturnsAsync(false);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdateCompanyClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Company client with id 0 not found or had been previously deleted.",exc.Message);
    }
    
    [Fact]
    public async Task UpdateCompanyClientInfoAsync_ThrowException_WhenEmailIsInvalid()
    {
        int id = 1;
        UpdateCompanyClientDTO updateClient = new UpdateCompanyClientDTO()
        {
            name = "Alot",
            address = "address 04",
            email = "alot",
            phoneNumber = "+47 123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsCompanyClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(updateClient.email)).Returns(-1);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdateCompanyClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Email is invalid. Please remember about @somemail.someending",exc.Message);
    }
    
    [Fact]
    public async Task UpdateCompanyClientInfoAsync_ThrowException_WhenPhoneNumberDoesntFitThePattern()
    {
        int id = 1;
        UpdateCompanyClientDTO updateClient = new UpdateCompanyClientDTO()
        {
            name = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47123123123"
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsCompanyClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(updateClient.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(updateClient.phoneNumber)).Returns(-2);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdateCompanyClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Phone number does not fit the pattern: +A B A - 1-3 digits, B - 9-11 digits",exc.Message);
    }
    
    [Fact]
    public async Task UpdateCompanyClientInfoAsync_ThrowException_WhenTransactionRollbacked()
    {
        int id = 1;
        UpdateCompanyClientDTO updateClient = new UpdateCompanyClientDTO()
        {
            name = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = null,
            CompanyId = 1
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsCompanyClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(updateClient.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(updateClient.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.GetClientAsync(id, token)).ReturnsAsync(client);
        _mockClientRepository.Setup(repo => repo.UpdateCompanyClientAsync(updateClient, client, token)).ReturnsAsync(-1);
        
        var exc = await Assert.ThrowsAsync<Exception>(() =>
            _clientService.UpdateCompanyClientInfoAsync(id, updateClient, token));
        Assert.Equal("Error: Transaction rollbacked.",exc.Message);
    }
    
    [Fact]
    public async Task UpdateCompanyClientInfoAsync_ReturnSuccess_WhenCompanyIsUpdated()
    {
        int id = 1;
        UpdateCompanyClientDTO updateClient = new UpdateCompanyClientDTO()
        {
            name = "Alot",
            address = "address 04",
            email = "alot@gmail.com",
            phoneNumber = "+47 123123123"
        };
        Client client = new Client()
        {
            ClientId = id,
            Address = "adres adres 05",
            Email = "kacper.alot5@gmail.com",
            IsDeleted = false,
            PhoneNumber = "48 123123123",
            PrivateClientId = null,
            CompanyId = 1
        };
        CancellationToken token = new CancellationToken();
        _mockClientRepository.Setup(repo => repo.IsCompanyClientInDbAsync(id, token)).ReturnsAsync(true);
        _mockClientRepository.Setup(repo => repo.EmailAddressValid(updateClient.email)).Returns(0);
        _mockClientRepository.Setup(repo => repo.DoPhoneNumberHaveAreaCode(updateClient.phoneNumber)).Returns(0);
        _mockClientRepository.Setup(repo => repo.GetClientAsync(id, token)).ReturnsAsync(client);
        _mockClientRepository.Setup(repo => repo.UpdateCompanyClientAsync(updateClient, client, token)).ReturnsAsync(0);
        
        var result = await _clientService.UpdateCompanyClientInfoAsync(id, updateClient, token);
        Assert.Equal($"Successfully updated company client with id 1.", result);
    }
}