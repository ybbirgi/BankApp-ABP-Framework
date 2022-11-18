using System;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Dtos.TransactionHistoryDtos;
using BankApp.Enums;
using BankApp.Services;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace BankApp;

public class TransactionHistoryAppServiceTests : BankAppApplicationTestBase
{
    private readonly ITransactionHistoryService _transactionHistoryService;
    private readonly TransactionHistoryCreateDto _transactionHistoryCreateDto;
    private Exception _exception;

    public TransactionHistoryAppServiceTests()
    {
        _transactionHistoryService = GetRequiredService<ITransactionHistoryService>();
        _transactionHistoryCreateDto = new TransactionHistoryCreateDto()
        {
            CardId = TestConstants.CreditCardId,
            Amount = 400,
            TransactionDirection = TransactionDirection.In,
            TransactionType = TransactionType.Eft,
            Definition = "Deposit"
        };
    }
    [Fact]
    public async Task Should_Create_Transaction_History()
    {
        _transactionHistoryCreateDto.TransactionDirection = TransactionDirection.Out;
        var result = await _transactionHistoryService.CreateAsync(_transactionHistoryCreateDto);
            
        result.Id.ShouldNotBe(Guid.Empty);
        result.CardId.ShouldBe(_transactionHistoryCreateDto.CardId);
    }

    [Fact]
    public async Task Should_NOT_Create_Transaction_History_Since_Card_NOT_Exists()
    {
        _transactionHistoryCreateDto.CardId = Guid.NewGuid();
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _transactionHistoryService.CreateAsync(_transactionHistoryCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }
    [Fact]
    public async Task Should_NOT_Create_Transaction_History_Since_Card_NOT_Have_Enough_Balance()
    {
        _transactionHistoryCreateDto.Amount = 50000;
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _transactionHistoryService.CreateAsync(_transactionHistoryCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.TransactionHistoryMessages.NotEnoughBalance);
    }

    [Fact]
    public async Task Should_NOT_Create_Deposit_Transaction_History_Since_Credit_Card_Has_No_Debt()
    {
        _transactionHistoryCreateDto.Amount = 1000;
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _transactionHistoryService.CreateAsync(_transactionHistoryCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.TransactionHistoryMessages.InvalidDepositTransaction);
    }

    [Fact]
    public async Task Should_Get_Transaction()
    {
        var result = await _transactionHistoryService.GetTransactionByIdAsync(TestConstants.TransactionId);
        
        result.Id.ShouldBe(TestConstants.TransactionId);
    }

    [Fact]
    public async Task Should_Not_Get_Transaction_Since_Transaction_Not_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionHistoryService.GetTransactionByIdAsync(Guid.NewGuid());
        });
        _exception.Message.ShouldBe(BusinessMessages.TransactionHistoryMessages.TransactionNotFound);
    }

    [Fact]
    public async Task Should_Get_All_Transactions()
    {
        var result = await _transactionHistoryService.GetAllTransactionsAsync();
        
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Get_All_Transactions_By_CardId()
    {
        var result = await _transactionHistoryService.GetAllTransactionsByCardIdAsync(TestConstants.CreditCardId);
        
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Should_NOT_Get_All_Transactions_By_CardId_Since_Card_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionHistoryService.GetAllTransactionsByCardIdAsync(Guid.NewGuid());
        });
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }

    [Fact]
    public async Task Should_Get_All_Transactions_By_Customer_Id()
    {
        var result = await _transactionHistoryService.GetAllTransactionsByCustomerIdAsync(TestConstants.CustomerId);

        await _transactionHistoryService.GetAllTransactionsByCustomerIdAsync(TestConstants.CustomerId);
    }
    
    [Fact]
    public async Task Should_NOT_Get_All_Transactions_By_CustomerId_Since_Customer_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _transactionHistoryService.GetAllTransactionsByCustomerIdAsync(Guid.NewGuid());
        });
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);
    }
}