using System;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Dtos.AccountDtos;
using BankApp.Enums;
using BankApp.Services;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace BankApp;

public sealed class AccountAppServiceTests : BankAppApplicationTestBase
{
    private readonly IAccountService _accountService;
    private readonly AccountCreateDto _accountCreateDto;
    private readonly AccountUpdateDto _accountUpdateDto;
    private Exception _exception;

    public AccountAppServiceTests()
    {
        _accountService = GetRequiredService<IAccountService>();
        _accountCreateDto = new AccountCreateDto()
        {
            CustomerId = TestConstants.CustomerId,
            AccountType = AccountType.VadeliAnadolu,
            Iban = "TR341234123412341234123424"
        };
        _accountUpdateDto = new AccountUpdateDto
        {
            CustomerId = TestConstants.CustomerId,
            AccountType = AccountType.VadesizAnadolu,
            Iban = "TR341234123412341234123411"
        };
    }

    [Fact]
    public async Task Should_Create_Account()
    {
        var result = await _accountService.CreateAsync(_accountCreateDto);
        result.Id.ShouldNotBe(Guid.Empty);
        result.CustomerId.ShouldBe(TestConstants.CustomerId);
    }
    [Fact]
    public async Task Should_NOT_Create_Account_Since_Iban_Is_NOT_Valid()
    {
        _accountCreateDto.Iban = "123";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.CreateAsync(_accountCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.IbanIsNotValid);
    }
    [Fact]
    public async Task Should_NOT_Create_Account_Since_Iban_Is_Used()
    {
        _accountCreateDto.Iban = "TR111111111111111111111111";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.CreateAsync(_accountCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.IbanIsAlreadyInUse);
    }

    [Fact]
    public async Task Should_Update_Customer()
    {
        var result = await _accountService.UpdateAsync(TestConstants.AccountId, _accountUpdateDto);
        
        result.Id.ShouldBe(TestConstants.AccountId);
        result.Iban.ShouldBe(_accountUpdateDto.Iban);
        result.AccountType.ShouldBe(_accountUpdateDto.AccountType);
        result.CustomerId.ShouldBe(TestConstants.CustomerId2);
    }

    [Fact]
    public async Task Should_NOT_Update_Customer_Since_Account_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.UpdateAsync(Guid.NewGuid(),_accountUpdateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_NOT_Update_Customer_Since_Iban_Is_Used()
    {
        _accountUpdateDto.Iban = "TR99999999999999999999999";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.UpdateAsync(TestConstants.AccountId,_accountUpdateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.IbanIsAlreadyInUse);
    }
    [Fact]
    public async Task Should_NOT_Update_Customer_Since_Iban_Is_Not_Valid()
    {
        _accountUpdateDto.Iban = "TR99999999999912999";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.UpdateAsync(TestConstants.AccountId,_accountUpdateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.IbanIsNotValid);
    }

    [Fact]
    public async Task Should_Delete_Account()
    {
        var result = await _accountService.DeleteAsync(TestConstants.AccountId);
        
        result.IsDeleted.ShouldBe(true);
    }
    [Fact]
    public async Task Should_NOT_Delete_Account_Since_Account_Not_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.DeleteAsync(Guid.NewGuid());
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_Get_Account()
    {
        var result = await _accountService.GetAccountAsync(TestConstants.AccountId);
        
        result.Id.ShouldBe(TestConstants.AccountId);
    }
    [Fact]
    public async Task Should_NOT_Get_Account_Since_Account_Not_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.GetAccountAsync(Guid.NewGuid());
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_Get_All_Accounts()
    {
        var result = await _accountService.GetAllAccountsAsync();
        
        result.Count.ShouldBe(2);
    }
    [Fact]
    public async Task Should_Get_All_Accounts_By_Customer_Id()
    {
        var result = await _accountService.GetAllByCustomerIdAsync(TestConstants.CustomerId);
        
        result.Count.ShouldBe(1);
    }
    [Fact]
    public async Task Should_NOT_Get_All_Accounts_By_Customer_Id_Since_Customer_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountService.GetAllByCustomerIdAsync(Guid.NewGuid());
        });

        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);
    }
}