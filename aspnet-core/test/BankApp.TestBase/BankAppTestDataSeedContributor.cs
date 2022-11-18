using System;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Entities;
using BankApp.Enums;
using BankApp.Repositories;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace BankApp;

public class BankAppTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICardRepository _cardRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private Customer _customer;
    private Customer _customer2;
    private Account _account;
    private Account _account2;
    private Card _creditCard;
    private Card _debitCard;
    private TransactionHistory _transactionHistory;
    public BankAppTestDataSeedContributor(ICustomerRepository customerRepository, IAccountRepository accountRepository, ICardRepository cardRepository, ITransactionHistoryRepository transactionHistoryRepository)
    {
        _customerRepository = customerRepository;
        _accountRepository = accountRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _cardRepository = cardRepository;
        _customer = new Customer()
        {
            Name = "Burak",
            LastName = "Birgi",
            IdentityNumber = "11111111111",
            BirthPlace = "Eskisehir",
            BirthDate = new DateTime(1998, 02, 16),
            RiskLimit = 10000
        };
        _customer2 = new Customer()
        {
            Name = "Murat",
            LastName = "Birgi",
            IdentityNumber = "22222222222",
            BirthPlace = "Eskisehir",
            BirthDate = new DateTime(1970, 02, 16),
            RiskLimit = 10000
        };
        _account = new Account()
        {
            CustomerId = TestConstants.CustomerId2,
            AccountType = AccountType.VadeliAnadolu,
            Iban = "TR111111111111111111111111"
        };
        _account2 = new Account()
        {
            CustomerId = TestConstants.CustomerId,
            AccountType = AccountType.VadeliAnadolu,
            Iban = "TR99999999999999999999999"
        };
        _creditCard = new Card()
        {
            AccountId = TestConstants.AccountId,
            CardType = CardType.Credit,
            CardNumber = "9999999999999999",
            Balance = 5000,
            Debt = 500
        };
        _debitCard = new Card()
        {
            AccountId = TestConstants.AccountId2,
            CardType = CardType.Debit,
            CardNumber = "1111111111111111",
            Balance = 0,
            Debt = 0
        };
        _transactionHistory = new TransactionHistory()
        {
            CustomerId = TestConstants.CustomerId,
            CardId = TestConstants.CreditCardId,
            Amount = 500,
            TransactionDirection = TransactionDirection.In,
            TransactionType = TransactionType.Eft,
            Definition = "Flat Rent",
            TransactionDate = DateTime.Now
        };
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        _customer.SetId(TestConstants.CustomerId);
        _customer2.SetId(TestConstants.CustomerId2);
        _account.SetId(TestConstants.AccountId);
        _account2.SetId(TestConstants.AccountId2);
        _creditCard.SetId(TestConstants.CreditCardId);
        _debitCard.SetId(TestConstants.DebitCardId);
        _transactionHistory.SetId(TestConstants.TransactionId);
        _customer2.RemainingRiskLimit = 5000;
        await _customerRepository.InsertAsync(_customer);
        await _customerRepository.InsertAsync(_customer2);
        await _accountRepository.InsertAsync(_account);
        await _accountRepository.InsertAsync(_account2);
        await _cardRepository.InsertAsync(_creditCard);
        await _cardRepository.InsertAsync(_debitCard);
        await _transactionHistoryRepository.InsertAsync(_transactionHistory);
    }
}
