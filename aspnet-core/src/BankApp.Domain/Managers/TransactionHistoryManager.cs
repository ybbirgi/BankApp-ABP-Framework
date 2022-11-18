using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Entities;
using BankApp.Enums;
using BankApp.Repositories;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace BankApp.Managers;

public class TransactionHistoryManager : DomainService
{
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly ICardRepository _cardRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;

    public TransactionHistoryManager(ITransactionHistoryRepository transactionHistoryRepository, ICardRepository cardRepository, IAccountRepository accountRepository, ICustomerRepository customerRepository)
    {
        _transactionHistoryRepository = transactionHistoryRepository;
        _cardRepository = cardRepository;
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
    }

    public async Task<TransactionHistory> CreateTransactionHistoryAsync(Guid cardId,float amount,TransactionDirection transactionDirection,
        TransactionType transactionType,string definition)
    {
        await CheckIfCardExists(cardId);
        
        var accountId = (await _cardRepository.FirstOrDefaultAsync(x => x.Id == cardId)).AccountId;
        var customerId = (await _accountRepository.FirstOrDefaultAsync(x => x.Id == accountId)).CustomerId;
        var cardType = (await _cardRepository.FirstOrDefaultAsync(x => x.Id == cardId)).CardType;
        await CheckIfCustomerHasEnoughBalanceAsync(cardId, amount);
        await UpdateCardBalanceAfterTransaction(cardId, amount, transactionDirection,cardType);
        if (cardType == CardType.Credit)
        {
            await CheckCreditCardDebtOnDepositTransactionsAsync(cardId,amount);
        }

        var transactionHistory =
            new TransactionHistory(customerId, cardId, amount,transactionDirection, transactionType, definition)
                {
                    TransactionDate = DateTime.Now
                };

        return transactionHistory;
    }

    public async Task<TransactionHistory> GetTransactionByIdAsync(Guid id)
    {
        await CheckIfTransactionExistsAsync(id);

        var transactionHistory = await _transactionHistoryRepository.FindAsync(id);

        return transactionHistory;
    }

    public async Task<List<TransactionHistory>> GetAllTransactionsAsync()
    {
        var transactionHistories = await _transactionHistoryRepository.GetListAsync();

        return transactionHistories;
    }

    public async Task<List<TransactionHistory>> GetAllTransactionsByCardIdAsync(Guid cardId)
    {
        await CheckIfCardExists(cardId);
        
        var transactionHistories = await _transactionHistoryRepository.GetListAsync(x => x.CardId == cardId);

        return transactionHistories;
    }

    public async Task<List<TransactionHistory>> GetAllTransactionsByCustomerIdAsync(Guid customerId)
    {
        await CheckIfCustomerExits(customerId);
        
        var transactionHistories = await _transactionHistoryRepository.GetListAsync(x => x.CardId == customerId);

        return transactionHistories;
    }

    private async Task CheckIfCardExists(Guid cardId)
    {
        if (await _cardRepository.FindAsync(cardId) == null)
        {
            throw new UserFriendlyException(BusinessMessages.CardMessages.CardNotFound);
        }
    }

    private async Task CheckIfCustomerExits(Guid customerId)
    {
        if (await _customerRepository.FindAsync(customerId) == null)
        {
            throw new UserFriendlyException(BusinessMessages.CustomerMessages.CustomerNotFound);
        }
    }
    
    private async Task CheckIfTransactionExistsAsync(Guid id)
    {
        if (await _transactionHistoryRepository.FindAsync(id) == null)
        {
            throw new UserFriendlyException(BusinessMessages.TransactionHistoryMessages.TransactionNotFound);
        }
    }

    private async Task CheckIfCustomerHasEnoughBalanceAsync(Guid cardId,float amount)
    {
        if ((await _cardRepository.FindAsync(cardId)).Balance < amount)
        {
            throw new UserFriendlyException(BusinessMessages.TransactionHistoryMessages.NotEnoughBalance);
        }
    }

    private async Task CheckCreditCardDebtOnDepositTransactionsAsync(Guid cardId, float amount)
    {
        if ((await _cardRepository.FindAsync(cardId)).Debt < amount)
        {
            throw new UserFriendlyException(BusinessMessages.TransactionHistoryMessages.InvalidDepositTransaction);
        }
    }
    private async Task CheckIfCreditCardHasDebt(Guid cardId)
    {
        if ((await _cardRepository.FindAsync(cardId)).Debt <= 0)
        {
            throw new UserFriendlyException(BusinessMessages.TransactionHistoryMessages.InvalidTransaction);
        }
    }
    private async Task UpdateCardBalanceAfterTransaction(Guid cardId, float amount,TransactionDirection transactionDirection,CardType cardType)
    {
        var card = await _cardRepository.FindAsync(cardId);
        if(transactionDirection == TransactionDirection.In)
        {
            card.Balance = card.Balance + amount;
            if (card.CardType == CardType.Credit)
            {
                await CheckIfCreditCardHasDebt(cardId);
                card.Debt = card.Debt - amount;
            }
        }
        else
        {
            card.Balance = card.Balance - amount;
            if (card.CardType == CardType.Credit)
            {
                card.Debt = card.Debt + amount;
            }
        }

        await _cardRepository.UpdateAsync(card);
    }
}