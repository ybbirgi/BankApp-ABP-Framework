using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Constants.Card;
using BankApp.Entities;
using BankApp.Enums;
using BankApp.Repositories;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace BankApp.Managers;

public class CardManager : DomainService
{
     private readonly ICardRepository _cardRepository;
     private readonly IAccountRepository _accountRepository;
     private readonly ICustomerRepository _customerRepository;

     public CardManager(ICardRepository cardRepository, IAccountRepository accountRepository, ICustomerRepository customerRepository)
    {
        _cardRepository = cardRepository;
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
    }
    //credit card
    public async Task<Card>CreateCardAsync(Guid accountId,string cardNumber,float balance)
    {
        cardNumber = CheckIfCardNumberContainsWhiteSpace(cardNumber);
        CheckIfCardNumberIsValid(cardNumber);
        await CheckIfCardNumberExistsAsync(cardNumber);
        
        var customerId = await GetCustomerIdFromAccountAsync(accountId);

        await CheckIfCustomerHasEnoughRemaningRiskLimitAsync(customerId, balance);
        await UpdateRemaningRiskLimitAfterCardsChangesAsync(customerId,balance);
        
        var card = new Card(accountId, CardType.Credit , cardNumber)
        {
            Balance = balance,
            Debt = 0
        };

        return card;
    }
    //debit card
    public async Task<Card>CreateCardAsync(Guid accountId,string cardNumber)
    {
        cardNumber = CheckIfCardNumberContainsWhiteSpace(cardNumber);
        CheckIfCardNumberIsValid(cardNumber);
        
        await CheckIfCardNumberExistsAsync(cardNumber);
        await CheckIfAccountAlreadyHasDebitCardAsync(accountId);
        await CheckIfAccountExistsAsync(accountId);

        var card = new Card(accountId, CardType.Debit , cardNumber)
        {
            Balance = 0,
            Debt = 0
        };

        return card;
    }
    public async Task<Card>UpdateCardAsync(Guid id,string cardNumber)
    {
        cardNumber = CheckIfCardNumberContainsWhiteSpace(cardNumber);
        await CheckIfCardExistsAsync(id);

        var card = await _cardRepository.FindAsync(id);
        
        if(card.CardNumber != cardNumber)
        {
            await CheckIfCardNumberExistsAsync(cardNumber);
            CheckIfCardNumberIsValid(cardNumber);
        }
        
        card = UpdateCardFields(card,cardNumber);
        
        return card;
    }

    public async Task<Card> DeleteCardAsync(Guid id)
    {
        await CheckIfCardExistsAsync(id);
        await CheckIfCardHasDebtAsync(id);
        
        var card = await _cardRepository.FindAsync(id);
        if (card.CardType == CardType.Credit)
        {
            var customerId = await GetCustomerIdFromAccountAsync((await _cardRepository.FindAsync(id)).AccountId);
            await UpdateRemaningRiskLimitAfterCardsChangesAsync(customerId, -card.Balance);
        }
        await _cardRepository.UpdateAsync(card);
        
        
        return card;
    }

    public async Task<Card> GetCardAsync(Guid id)
    {
        await CheckIfCardExistsAsync(id);

        var card = await _cardRepository.FindAsync(id);
        
        return card;
    }

    public async Task<List<Card>> GetAllCardsAsync()
    {
        var cards = await _cardRepository.GetListAsync();

        return cards;
    }

    public async Task<List<Card>> GetAllByAccountIdAsync(Guid accountId)
    {
        await CheckIfAccountExistsAsync(accountId);
        
        var cards = await _cardRepository.GetListAsync(x => x.AccountId == accountId);

        return cards;
    }

    private async Task<Guid> GetCustomerIdFromAccountAsync(Guid accountId)
    {
        var account = await _accountRepository.FirstOrDefaultAsync(x => x.Id == accountId);
        if (account == null)
        {
            throw new UserFriendlyException(BusinessMessages.AccountMessages.AccountNotFound);
        }

        return account.CustomerId;
    }

    private async Task CheckIfCardExistsAsync(Guid id)
    {
        if (await _cardRepository.FindAsync(id) == null)
        {
            throw new UserFriendlyException(BusinessMessages.CardMessages.CardNotFound);
        }
    }

    private async Task CheckIfCustomerHasEnoughRemaningRiskLimitAsync(Guid customerId,float balance)
    {
        if (balance > (await _customerRepository.FirstOrDefaultAsync(x => x.Id == customerId)).RemainingRiskLimit)
        {
            throw new UserFriendlyException(BusinessMessages.CardMessages.RiskLimitExceeded);
        }
    }

    private async Task CheckIfAccountAlreadyHasDebitCardAsync(Guid accountId)
    {
        if (await _cardRepository.FirstOrDefaultAsync(x => x.AccountId == accountId && x.CardType == CardType.Debit) != null)
        {
            throw new UserFriendlyException(BusinessMessages.CardMessages.AlreadyHaveDebitCard);
        }
    }

    private async Task CheckIfAccountExistsAsync(Guid accountId)
    {
        if (await _accountRepository.FirstOrDefaultAsync(x=> x.Id == accountId) == null)
        {
            throw new UserFriendlyException(BusinessMessages.AccountMessages.AccountNotFound);
        }
    }

    private async Task CheckIfCardHasDebtAsync(Guid id)
    {
        if ((await _cardRepository.FirstOrDefaultAsync(x => x.Id == id)).Debt > 0)
        {
            throw new UserFriendlyException(BusinessMessages.CardMessages.PayDebtFirst);
        }
    }
    public async Task CheckIfCardNumberExistsAsync(string cardNumber)
    {
        if ((await _cardRepository.FindAsync(x => x.CardNumber == cardNumber)) != null)
        {
            throw new UserFriendlyException(BusinessMessages.CardMessages.CardNumberIsInUse);
        }
    }

    private static void CheckIfCardNumberIsValid(string cardNumber)
    {
        if (cardNumber.Length != CardConstants.CardNumberLenght - 3)
        {
            throw new UserFriendlyException(BusinessMessages.CardMessages.CardNumberIsNotValid);
        }
    }
    private static string CheckIfCardNumberContainsWhiteSpace(string cardNumber)
    {
        if (cardNumber.Contains(' '))
        {
            cardNumber = RemoveWhiteSpaces(cardNumber);
        }

        return cardNumber;
    }
    
    private static string RemoveWhiteSpaces(string cardNumber)
    {
        cardNumber = string.Join("", cardNumber.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        return cardNumber;
    }

    private static Card UpdateCardFields(Card card, string cardNumber)
    {
        card.CardNumber = cardNumber;
        return card;
    }
    private async Task UpdateRemaningRiskLimitAfterCardsChangesAsync(Guid customerId,float amount)
    {
        var customer = await _customerRepository.FindAsync(customerId);
        customer.RemainingRiskLimit = customer.RemainingRiskLimit - amount;
        await _customerRepository.UpdateAsync(customer);
    }
}