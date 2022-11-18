using System;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Dtos.CardDtos;
using BankApp.Enums;
using BankApp.Services;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace BankApp;

public sealed class CardAppServiceTests : BankAppApplicationTestBase
{
    private ICardService _cardService;
    private Exception _exception;
    private CreditCardCreateDto _creditCardCreateDto;
    private DebitCardCreateDto _debitCardCreateDto;
    private CardUpdateDto _cardUpdateDto;

    public CardAppServiceTests()
    {
        _cardService = GetRequiredService<ICardService>();
        _creditCardCreateDto = new CreditCardCreateDto()
        {
            AccountId = TestConstants.AccountId,
            CardNumber = "1234123412341234",
            Balance = 5000
        };
        _debitCardCreateDto = new DebitCardCreateDto()
        {
            AccountId = TestConstants.AccountId,
            CardNumber = "4321432143214321"
        };
        _cardUpdateDto = new CardUpdateDto()
        {
            CardNumber = "5555555555555555"
        };
    }

    [Fact]
    public async Task Should_Create_Credit_Card()
    {
        var result = await _cardService.CreateCreditCardAsync(_creditCardCreateDto);
        
        result.Id.ShouldNotBe(Guid.Empty);
        result.CardType.ShouldBe(CardType.Credit);
        result.AccountId.ShouldBe(TestConstants.AccountId);
    }

    [Fact]
    public async Task Should_NOT_Create_Credit_Card_Since_Account_NOT_Exist()
    {
        _creditCardCreateDto.AccountId = Guid.NewGuid();
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateCreditCardAsync(_creditCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_NOT_Create_Credit_Card_Since_CardNumber_Is_NOT_Valid()
    {
        _creditCardCreateDto.CardNumber = "1234";
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateCreditCardAsync(_creditCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsNotValid);
    }
    [Fact]
    public async Task Should_NOT_Create_Credit_Card_Since_CardNumber_In_Use()
    {
        _creditCardCreateDto.CardNumber = "9999999999999999";
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateCreditCardAsync(_creditCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsInUse);
    }

    [Fact]
    public async Task Should_NOT_Create_Credit_Card_Since_Customer_NOT_Has_Enough_Risk_Limit_Remaining()
    {
        _creditCardCreateDto.Balance = 6000;
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateCreditCardAsync(_creditCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.RiskLimitExceeded);
    }

    [Fact]
    public async Task Should_Create_Debit_Card()
    {
        var result = await _cardService.CreateDebitCardAsync(_debitCardCreateDto);
        
        result.Id.ShouldNotBe(Guid.Empty);
        result.CardType.ShouldBe(CardType.Debit);
        result.AccountId.ShouldBe(TestConstants.AccountId);
    }
    [Fact]
    public async Task Should_NOT_Create_Debit_Card_Since_Account_NOT_Exist()
    {
        _debitCardCreateDto.AccountId = Guid.NewGuid();
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateDebitCardAsync(_debitCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
    [Fact]
    public async Task Should_NOT_Create_Debit_Card_Since_CardNumber_Is_NOT_Valid()
    {
        _debitCardCreateDto.CardNumber = "1234";
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateDebitCardAsync(_debitCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsNotValid);
    }
    [Fact]
    public async Task Should_NOT_Create_Debit_Card_Since_CardNumber_In_Use()
    {
        _debitCardCreateDto.CardNumber = "9999999999999999";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateDebitCardAsync(_debitCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsInUse);
    }
    [Fact]
    public async Task Should_NOT_Create_Debit_Card_Since_Customer_Already_Has_Debit_Card()
    {
        _debitCardCreateDto.AccountId = TestConstants.AccountId2;
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.CreateDebitCardAsync(_debitCardCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.AlreadyHaveDebitCard);
    }
    [Fact]
    public async Task Should_Update_Card()
    {
        var result = await _cardService.UpdateAsync(TestConstants.CreditCardId, _cardUpdateDto);
        
        result.CardNumber.ShouldBe("5555555555555555");
    }
    [Fact]
    public async Task Should_NOT_Update_Card_Since_Card_NOT_Exists()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.UpdateAsync(Guid.NewGuid(),_cardUpdateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }
    [Fact]
    public async Task Should_NOT_Update_Card_Since_CardNumber_Is_Used()
    {
        _cardUpdateDto.CardNumber = "1111111111111111";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.UpdateAsync(TestConstants.CreditCardId,_cardUpdateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsInUse);
    }
    [Fact]
    public async Task Should_NOT_Update_Card_Since_CardNumber_NOT_Valid()
    {
        _cardUpdateDto.CardNumber = "123";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.UpdateAsync(TestConstants.CreditCardId,_cardUpdateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNumberIsNotValid);
    }

    [Fact]
    public async Task Should_Delete_Card()
    {
        var result = await _cardService.DeleteAsync(TestConstants.DebitCardId);
        
        result.IsDeleted.ShouldBe(true);
        
    }
    [Fact]
    public async Task Should_NOT_Delete_Card_Since_Card_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.DeleteAsync(Guid.NewGuid());
        });
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
        
    }
    [Fact]
    public async Task Should_NOT_Delete_Card_Since_Card_Has_Debt()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.DeleteAsync(TestConstants.CreditCardId);
        });
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.PayDebtFirst);
        
    }

    [Fact]
    public async Task Should_Get_Card()
    {
        var result = await _cardService.GetCardAsync(TestConstants.CreditCardId);
        
        result.Id.ShouldBe(TestConstants.CreditCardId);
    }
    [Fact]
    public async Task Should_NOT_Get_Card_Since_Card_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.GetCardAsync(Guid.NewGuid());
        });
        _exception.Message.ShouldBe(BusinessMessages.CardMessages.CardNotFound);
    }

    [Fact]
    public async Task Should_Get_All_Cards()
    {
        var result = await _cardService.GetAllCardsAsync();
        
        result.Count.ShouldBe(2);
    }
    
    [Fact]
    public async Task Should_Get_All_Cards_By_Account_Id()
    {
        var result = await _cardService.GetAllByAccountId(TestConstants.AccountId);
        
        result.Count.ShouldBe(1);
    }
    [Fact]
    public async Task Should_NOT_Get_All_Cards_By_Account_Id_Since_Account_Not_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _cardService.GetAllByAccountId(Guid.NewGuid());
        });
        _exception.Message.ShouldBe(BusinessMessages.AccountMessages.AccountNotFound);
    }
}
