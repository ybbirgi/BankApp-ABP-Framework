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

public class CardManagerTests : BankAppDomainTestBase
{
    private readonly ICardRepository _fakeRepoCard;
    private readonly IAccountRepository _fakeRepoAccount;
    private readonly ICustomerRepository _fakeRepoCustomer;
    private readonly CardManager _cardManager;
    private readonly Guid _cardId;
    private readonly Guid _accountId;
    private readonly Guid _customerId;
    private readonly Card _creditCard;
    private readonly Card _debitCard;
    private readonly Card _debitCard2;
    private readonly Account _account;
    private readonly Customer _customer;
    private Exception _exception;

    public CardManagerTests()
    {
        _fakeRepoCustomer = Substitute.For<ICustomerRepository>();
        _fakeRepoAccount = Substitute.For<IAccountRepository>();
        _fakeRepoCard = Substitute.For<ICardRepository>();
        _cardManager = new CardManager(_fakeRepoCard,_fakeRepoAccount,_fakeRepoCustomer);
        _cardId = Guid.NewGuid();
        _accountId = Guid.NewGuid();
        _customerId = Guid.NewGuid();
        _creditCard = new Card(_accountId, CardType.Credit, "1111222233334444");
        _debitCard = new Card(_accountId, CardType.Debit, "4444333322221111");
        _debitCard2 = new Card(_accountId, CardType.Debit, "4444333322224444");
        _account = new Account(_customerId, AccountType.VadeliAnadolu, "TR23 1234 1234 1234 1234 1234 23");
        _customer = new Customer("Yusuf Besim", "Birgi", "12345678952", "Eskisehir", DateTime.Now, 10000);
    }

    [Fact]
    public async Task Should_Create_A_Credit_Card()
    {
        _fakeRepoCard.FirstOrDefaultAsync(x => x.CardNumber == _creditCard.CardNumber).Returns(null as Card);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);
        _fakeRepoCustomer.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(_customer);
        _fakeRepoCustomer.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        
        var createdCreditCard = await _cardManager.CreateCardAsync(_accountId, _creditCard.CardNumber, 5000);
        
        createdCreditCard.ShouldNotBeNull();
        _customer.RemainingRiskLimit.ShouldBe(5000);
    }
    [Fact]
    public async Task Should_NOT_Create_A_Credit_Card_Since_CardNumber_NOT_Valid()
    {
        _creditCard.CardNumber = "123";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _creditCard.CardNumber, 5000);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsNotValid);
    
    }
    [Fact]
    public async Task Should_NOT_Create_A_Credit_Card_Since_CardNumber_Is_Used()
    {
        _fakeRepoCard.FindAsync(x => x.CardNumber == _creditCard.CardNumber).ReturnsForAnyArgs(_creditCard);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _creditCard.CardNumber, 5000);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsInUse);
    }
    [Fact]
    public async Task Should_NOT_Create_A_Credit_Card_Since_Account_Does_NOT_Exist()
    {
        _fakeRepoCard.FirstOrDefaultAsync(x => x.CardNumber == _creditCard.CardNumber).Returns(null as Card);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).Returns(null as Account);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _creditCard.CardNumber, 5000);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    
    [Fact]
    public async Task Should_NOT_Create_A_Credit_Card_Since_Customer_Remaining_Risk_Limit_NOT_Enough()
    {
        _fakeRepoCard.FirstOrDefaultAsync(x => x.CardNumber == _creditCard.CardNumber).Returns(null as Card);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);
        _fakeRepoCustomer.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(_customer);
        _fakeRepoCustomer.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _creditCard.CardNumber, 15000);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.RiskLimitExceeded);
    }
    [Fact]
    public async Task Should_Create_A_Debit_Card()
    {
        _fakeRepoCard.FirstOrDefaultAsync(x => x.CardNumber == _debitCard.CardNumber).Returns(null as Card);
        _fakeRepoCard.FirstOrDefaultAsync(x => x.AccountId == _accountId && x.CardType == CardType.Debit).Returns(null as Card);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);

        var createdDebitCard = await _cardManager.CreateCardAsync(_accountId, _debitCard.CardNumber);
        
        createdDebitCard.ShouldNotBeNull();
    }
    [Fact]
    public async Task Should_NOT_Create_A_Debit_Card_Since_CardNumber_NOT_Valid()
    {
        _debitCard.CardNumber = "123";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _debitCard.CardNumber);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsNotValid);
    }
    [Fact]
    public async Task Should_NOT_Create_A_Debit_Card_Since_CardNumber_Is_Used()
    {
        _fakeRepoCard.FindAsync(x => x.CardNumber == _debitCard.CardNumber).ReturnsForAnyArgs(_debitCard);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _debitCard.CardNumber);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsInUse);
    }
    [Fact]
    public async Task Should_NOT_Create_A_Debit_Card_Since_Account_Does_NOT_Exist()
    {
        _fakeRepoCard.FirstOrDefaultAsync(x => x.CardNumber == _debitCard.CardNumber).Returns(null as Card);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).Returns(null as Account);
        _exception = await Assert.ThrowsAsync
            <UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _debitCard.CardNumber);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_NOT_Create_A_Debit_Card_Since_Account_AlreadyHasDebitCard()
    {
        _fakeRepoCard.FindAsync(x => x.CardNumber == _debitCard.CardNumber).Returns(null as Card);
        _fakeRepoCard.FirstOrDefaultAsync(x => x.AccountId == _accountId && x.CardType == CardType.Debit).ReturnsForAnyArgs(_debitCard2);
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.CreateCardAsync(_accountId, _debitCard.CardNumber);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.AlreadyHaveDebitCard);
    }
    [Fact]
    public async Task Should_Update_Card()
    {
        _fakeRepoCard.FirstOrDefaultAsync(x => x.CardNumber == _creditCard.CardNumber).Returns(null as Card);
        _fakeRepoCard.FindAsync(_cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).Returns(null as Account);

        var updatedCard = await _cardManager.UpdateCardAsync(_cardId, "1234 4321 1234 4321");
        
        updatedCard.CardNumber.ShouldBe("1234432112344321");
        
    }
    [Fact]
    public async Task Should_NOT_Update_Card_Since_Card_Does_NOT_Exist()
    {
        _fakeRepoCard.FindAsync(_cardId).Returns(null as Card);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.UpdateCardAsync(_cardId, _creditCard.CardNumber);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }
    [Fact]
    public async Task Should_NOT_Update_Card_Since_CardNumber_Is_Used()
    {
        _fakeRepoCard.FindAsync(_cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoCard.FindAsync(x => x.CardNumber == _creditCard.CardNumber).ReturnsForAnyArgs(_debitCard);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.UpdateCardAsync(_cardId, _debitCard.CardNumber);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsInUse);
    }
    [Fact]
    public async Task Should_NOT_Update_Card_Since_CardNumber_Is_Not_Valid()
    {
        _fakeRepoCard.FindAsync(_cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoCard.FirstOrDefaultAsync(x => x.CardNumber == _creditCard.CardNumber).Returns(null as Card);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.UpdateCardAsync(_cardId, "1234");
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsNotValid);
    }
    [Fact]
    public async Task Should_Delete_Credit_Card()
    {
        _customer.RemainingRiskLimit = 5000;
        _creditCard.Balance = 5000;
        _creditCard.Debt = 0;
        _fakeRepoCard.FindAsync(_cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoCard.FirstOrDefaultAsync(x => x.Id == _cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);
        _fakeRepoCustomer.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(_customer);
        _fakeRepoCustomer.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        
        var deletedCard = await _cardManager.DeleteCardAsync(_cardId);

        deletedCard.ShouldNotBeNull();
        _customer.RemainingRiskLimit.ShouldBe(10000);
    }
    [Fact]
    public async Task Should_NOT_Delete_Credit_Card_Since_Card_NOT_Exists()
    {
        _fakeRepoCard.FindAsync(_cardId).Returns(null as Card);
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.DeleteCardAsync(_cardId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }
    [Fact]
    public async Task Should_NOT_Delete_Credit_Card_Since_Card_Has_Debt()
    {
        _customer.RemainingRiskLimit = 5000;
        _creditCard.Balance = 4000;
        _creditCard.Debt = 1000;
        _fakeRepoCard.FindAsync(_cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoCard.FirstOrDefaultAsync(x => x.Id == _cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);
        _fakeRepoCustomer.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(_customer);
        _fakeRepoCustomer.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.DeleteCardAsync(_cardId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.PayDebtFirst);
    }
    [Fact]
    public async Task Should_NOT_Delete_Credit_Card_Since_Account_NOT_Found()
    {
        _fakeRepoCard.FindAsync(_cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoCard.FirstOrDefaultAsync(x => x.Id == _cardId).ReturnsForAnyArgs(_creditCard);
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).Returns(null as Account);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.DeleteCardAsync(_cardId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_Get_Card()
    {
        _fakeRepoCard.FindAsync(_cardId).ReturnsForAnyArgs(_creditCard);
        
        var listedCard = await _cardManager.GetCardAsync(_cardId);
        
        listedCard.ShouldBeSameAs(_creditCard);
    }
    [Fact]
    public async Task Should_NOT_Get_Card_Since_Card_Not_Exist()
    {
        _fakeRepoCard.FindAsync(_cardId).Returns(null as Card);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.GetCardAsync(_cardId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }
    [Fact]
    public async Task Should_Get_All_Cards()
    {
        var cardList = new List<Card> { _creditCard, _debitCard };

        _fakeRepoCard.GetListAsync().ReturnsForAnyArgs(cardList);

        var listedCards = await _cardManager.GetAllCardsAsync();
        
        listedCards.ShouldBeSameAs(cardList);
    }
    [Fact]
    public async Task Should_Get_All_Cards_Of_An_Account()
    {
        var cardList = new List<Card> { _creditCard, _debitCard };
        
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);
        _fakeRepoCard.GetListAsync(x=> x.AccountId == _accountId).ReturnsForAnyArgs(cardList);
        
        var listedCards = await _cardManager.GetAllByAccountIdAsync(_accountId);
        
        listedCards.ShouldBeSameAs(cardList);
    }
    [Fact]
    public async Task Should_NOT_Get_All_Cards_Of_An_Account_Since_Account_NOT_Exist()
    {
        var cardList = new List<Card> { _creditCard, _debitCard };
        
        _fakeRepoAccount.FirstOrDefaultAsync(x => x.Id == _accountId).Returns(null as Account);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardManager.GetAllByAccountIdAsync(_cardId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    
    
}