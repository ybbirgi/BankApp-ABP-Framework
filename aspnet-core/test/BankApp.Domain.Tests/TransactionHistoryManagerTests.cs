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

public class TransactionHistoryManagerTests
{
    private readonly ITransactionHistoryRepository _fakeTransactionHistoryRepository;
    private readonly ICardRepository _fakeCardRepository;
    private readonly IAccountRepository _fakeAccountRepository;
    private readonly ICustomerRepository _fakeCustomerRepository;
    private readonly TransactionHistoryManager _transactionManager;
    private readonly Guid _cardId;
    private readonly Guid _accountId;
    private readonly Guid _customerId;
    private readonly Guid _transactionHistoryId;
    private readonly Account _account;
    private readonly Card _card;
    private readonly TransactionHistory _transactionHistory;
    private readonly Customer _customer;
    private Exception _exception;
    public TransactionHistoryManagerTests()
    {
        _fakeCardRepository = Substitute.For<ICardRepository>();
        _fakeTransactionHistoryRepository = Substitute.For<ITransactionHistoryRepository>();
        _fakeAccountRepository = Substitute.For<IAccountRepository>();
        _fakeCustomerRepository = Substitute.For<ICustomerRepository>();
        _transactionManager = new TransactionHistoryManager(_fakeTransactionHistoryRepository, _fakeCardRepository, _fakeAccountRepository,_fakeCustomerRepository);
        _cardId = Guid.NewGuid();
        _accountId = Guid.NewGuid();
        _customerId = Guid.NewGuid();
        _transactionHistoryId = Guid.NewGuid();
        _account = new Account(_customerId, AccountType.VadeliAnadolu, "TR23 1234 1234 1234 1234 1234 23");
        _card = new Card(_accountId, CardType.Credit, "1111 2222 3333 4444");
        _transactionHistory = new TransactionHistory(_customerId, _cardId, 500, TransactionDirection.Out,
            TransactionType.Eft, "Spending");
        _customer = new Customer("Yusuf Besim", "Birgi", "12345678952", "Eskisehir", DateTime.Now, 10000);

    }
    
    [Fact]
    public async Task Should_Create_A_Withdraw_Transaction_History()
    {
        _card.Balance = 9500;
        _card.Debt = 500;
        _fakeCardRepository.FirstOrDefaultAsync(x => x.Id == _cardId).ReturnsForAnyArgs(_card);
        _fakeAccountRepository.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);
        _fakeCardRepository.FindAsync(_cardId).ReturnsForAnyArgs(_card);

        var newTransactionHistory = await _transactionManager.CreateTransactionHistoryAsync(_cardId, 500, TransactionDirection.Out,
            TransactionType.Fast, "Spending");

        newTransactionHistory.ShouldNotBeNull();
        _card.Debt.ShouldBe(1000);
        _card.Balance.ShouldBe(9000);
    }
    [Fact]
    public async Task Should_NOT_Create_A_Withdraw_Transaction_History_Since_Card_NOT_Exist()
    {
        _fakeCardRepository.FirstOrDefaultAsync(x => x.Id == _cardId).Returns(null as Card);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionManager.CreateTransactionHistoryAsync(_transactionHistory.CardId, _transactionHistory.Amount, 
                _transactionHistory.TransactionDirection, _transactionHistory.TransactionType, _transactionHistory.Definition);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }
    [Fact]
    public async Task Should_NOT_Create_A_Withdraw_Transaction_History_Since_Not_Enough_Balance()
    {
        _card.Balance = 9500;
        _card.Debt = 500;
        _transactionHistory.Amount = 10000;
        _fakeCardRepository.FirstOrDefaultAsync(x => x.Id == _cardId).ReturnsForAnyArgs(_card);
        _fakeAccountRepository.FirstOrDefaultAsync(x => x.Id == _accountId).ReturnsForAnyArgs(_account);
        _fakeCardRepository.FindAsync(_cardId).ReturnsForAnyArgs(_card);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionManager.CreateTransactionHistoryAsync(_transactionHistory.CardId, _transactionHistory.Amount, 
                _transactionHistory.TransactionDirection, _transactionHistory.TransactionType, _transactionHistory.Definition);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.TransactionHistoryMessages.NotEnoughBalance);
    }
    [Fact]
    public async Task Should_Get_Transaction()
    {
        _fakeTransactionHistoryRepository.FindAsync(_transactionHistoryId).ReturnsForAnyArgs(_transactionHistory);

        var listedTransactionHistory = await _transactionManager.GetTransactionByIdAsync(_transactionHistoryId);
        
        listedTransactionHistory.ShouldBeSameAs(_transactionHistory);
    }
    [Fact]
    public async Task Should_Not_Get_Transaction_Since_Transaction_Not_Exist()
    {
        _fakeTransactionHistoryRepository.FindAsync(_transactionHistoryId).Returns(null as TransactionHistory);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionManager.GetTransactionByIdAsync(_transactionHistoryId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.TransactionHistoryMessages.TransactionNotFound);
    }
    [Fact]
    public async Task Should_Get_All_Transactions()
    {
        var transaction2 = new TransactionHistory(_customerId, _cardId, 500, TransactionDirection.In,
            TransactionType.Eft, "Deposit");
        
        var transactionHistoryList = new List<TransactionHistory> { _transactionHistory, transaction2 };

        _fakeTransactionHistoryRepository.GetListAsync().ReturnsForAnyArgs(transactionHistoryList);
        
        var listedTransactionHistories = await _transactionManager.GetAllTransactionsAsync();
        
        listedTransactionHistories.ShouldBeSameAs(transactionHistoryList);
    }
    [Fact]
    public async Task Should_Get_All_Transactions_By_Card_Id()
    {
        var transaction2 = new TransactionHistory(_customerId, _cardId, 500, TransactionDirection.In,
            TransactionType.Eft, "Deposit");
        
        var transactionHistoryList = new List<TransactionHistory> { _transactionHistory, transaction2 };

        _fakeCustomerRepository.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeCardRepository.FindAsync(_cardId).ReturnsForAnyArgs(_card);
        _fakeTransactionHistoryRepository.GetListAsync(x=>x.CardId == _cardId).ReturnsForAnyArgs(transactionHistoryList);
        
        var listedTransactionHistories = await _transactionManager.GetAllTransactionsByCardIdAsync(_cardId);
        
        listedTransactionHistories.ShouldBeSameAs(transactionHistoryList);
    }
    [Fact]
    public async Task Should_NOT_Get_All_Transactions_By_Card_Id_Since_Card_NOT_Exist()
    {
        _fakeCardRepository.FindAsync(_cardId).Returns(null as Card);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionManager.GetAllTransactionsByCardIdAsync(_cardId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }
    [Fact]
    public async Task Should_Get_All_Transactions_By_Customer_Id()
    {
        var transaction2 = new TransactionHistory(_customerId, _cardId, 500, TransactionDirection.In,
            TransactionType.Eft, "Deposit");
        
        var transactionHistoryList = new List<TransactionHistory> { _transactionHistory, transaction2 };

        _fakeCustomerRepository.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeCardRepository.FindAsync(_cardId).ReturnsForAnyArgs(_card);
        _fakeTransactionHistoryRepository.GetListAsync(x=>x.CardId == _cardId).ReturnsForAnyArgs(transactionHistoryList);
        
        var listedTransactionHistories = await _transactionManager.GetAllTransactionsByCustomerIdAsync(_customerId);
        
        listedTransactionHistories.ShouldBeSameAs(transactionHistoryList);
    }
    [Fact]
    public async Task Should_NOT_Get_All_Transactions_By_Card_Id_Since_Customer_NOT_Exist()
    {
        _fakeCustomerRepository.FindAsync(_customerId).Returns(null as Customer);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionManager.GetAllTransactionsByCustomerIdAsync(_customerId);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);
    }
}