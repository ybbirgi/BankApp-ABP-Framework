using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Entities;
using BankApp.Enums;
using BankApp.Managers;
using BankApp.Repositories;
using NSubstitute;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace BankApp;

public class AccountManagerTests : BankAppDomainTestBase
{
    private readonly IAccountRepository _fakeRepo;
    private readonly ICustomerRepository _fakeCustomerRepository;
    private readonly AccountManager _accountManager;
    private readonly Guid _accountId;
    private readonly Guid _customerId;
    private readonly Account _account;
    private readonly Customer _customer;
    private Exception _exception;
    

    public AccountManagerTests()
    {
        _fakeRepo = Substitute.For<IAccountRepository>();
        _fakeCustomerRepository = Substitute.For<ICustomerRepository>();
        _accountManager = new AccountManager(_fakeRepo,_fakeCustomerRepository);
        _accountId = Guid.NewGuid();
        _customerId = Guid.NewGuid();
        _account = new Account(_customerId,AccountType.VadeliAnadolu,"TR 4444 5555 4444 3333 2222 1111");
        _customer = new Customer("Yusuf Besim", "Birgi", "12345678952", "Eskisehir", DateTime.Now, 10000);

    }

    [Fact]
    public async Task Should_Create_Account()
    {
        _fakeRepo.FirstOrDefaultAsync(x => x.Iban == _account.Iban).Returns(null as Account);

        var newAccount =
            await _accountManager.CreateAccountAsync(_account.CustomerId, _account.AccountType, _account.Iban);
        
        newAccount.ShouldNotBeNull();
    }
    [Fact]
    public async Task Should_NOT_Create_Account_Since_Iban_Is_Used()
    {
        _fakeRepo.FirstOrDefaultAsync(x => x.Iban == _account.Iban).ReturnsForAnyArgs(new Account(Guid.NewGuid(),AccountType.VadeliAnadolu,"TR444455554444333322221111"));

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountManager.CreateAccountAsync(_account.CustomerId, _account.AccountType, _account.Iban);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.IbanIsAlreadyInUse);
        
    }
    [Fact]
    public async Task Should_NOT_Create_Account_Since_Iban_Not_Valid()
    {
        _account.Iban = "TR123123";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountManager.CreateAccountAsync(_account.CustomerId, _account.AccountType, _account.Iban);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.IbanIsNotValid);
    }
    [Fact]
    public async Task Should_Update_Account()
    {
        _fakeRepo.FindAsync(_accountId).ReturnsForAnyArgs(_account);

        var updatedAccount =
            await _accountManager.UpdateAccountAsync(_accountId, AccountType.VadeliAnadolu , "TR 1111 5555 4444 3333 2222 1111");
        
        updatedAccount.AccountType.ShouldBe(AccountType.VadeliAnadolu);
        updatedAccount.Iban.ShouldBe("TR111155554444333322221111");
    }
    [Fact]
    public async Task Should_NOT_Update_Account_Since_Account_Does_NOT_Exist()
    {
        _fakeRepo.FindAsync(_accountId).Returns(null as Account);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountManager.UpdateAccountAsync(_account.CustomerId, AccountType.VadeliAnadolu, _account.Iban);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }

    [Fact]
    public async Task Should_NOT_Update_Account_Since_Iban_Is_Used()
    {
        _fakeRepo.FindAsync(_accountId).ReturnsForAnyArgs(_account);
        _fakeRepo.FirstOrDefaultAsync(x => x.Iban == _account.Iban).ReturnsForAnyArgs(new Account(Guid.NewGuid(),AccountType.VadeliAnadolu,"TR444455554444333322224444"));
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountManager.UpdateAccountAsync(_account.CustomerId, _account.AccountType,"TR444455554444333322224444");
        });
        
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.IbanIsAlreadyInUse);
    }

    [Fact]
    public async Task Should_Delete_Account()
    {
        _fakeRepo.FindAsync(_accountId).ReturnsForAnyArgs(_account);

        var deletedAccount = await _accountManager.DeleteAccountAsync(_accountId);
        
        deletedAccount.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task Should_NOT_Delete_Account_Since_Account_Does_NOT_Exist()
    {
        _fakeRepo.FindAsync(_accountId).Returns(null as Account);
 
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountManager.DeleteAccountAsync(_accountId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
        
    }

    [Fact]
    public async Task Should_Get_Account()
    {
        _fakeRepo.FindAsync(_accountId).ReturnsForAnyArgs(_account);

        var listedAccount = await _accountManager.GetAccountAsync(_accountId);
        
        listedAccount.ShouldBe(_account);
    }
    [Fact]
    public async Task Should_Not_Get_Account_Since_Account_Does_NOT_Exist()
    {
        _fakeRepo.FindAsync(_accountId).Returns(null as Account);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _accountManager.GetAccountAsync(_accountId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_Get_All_Accounts()
    {
        var account2 = new Account(_accountId, AccountType.VadesizAnadolu, "TR56 4444 5555 6666 3333 2222 44");
        var accountList = new List<Account> { _account, account2 };

        _fakeRepo.GetListAsync().ReturnsForAnyArgs(accountList);

        var accountsListed = await _accountManager.GetAllAccountsAsync();
        
        accountList.ShouldBeSameAs(accountsListed);
    }

    [Fact]
    public async Task Should_Get_All_Accounts_By_Customer_Id()
    {
        var account2 = new Account(_accountId, AccountType.VadesizAnadolu, "TR56 4444 5555 6666 3333 2222 44");
        var accountList = new List<Account> { _account, account2 };
        
        _fakeRepo.GetListAsync(x => x.CustomerId == _customerId).ReturnsForAnyArgs(accountList);
        _fakeCustomerRepository.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(_customer);
        
        var accountsListed = await _accountManager.GetAllByCustomerIdAsync(_customerId);
        
        accountList.ShouldBeSameAs(accountsListed);
    }
    
}