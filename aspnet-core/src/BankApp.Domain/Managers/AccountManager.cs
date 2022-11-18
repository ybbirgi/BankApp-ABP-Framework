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

public class AccountManager : DomainService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;

    public AccountManager(IAccountRepository accountRepository, ICustomerRepository customerRepository)
    {
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Account>CreateAccountAsync(Guid customerId,AccountType accountType,string iban)
    {
        iban = CheckIfIbanContainsWhiteSpace(iban);
        CheckIfIbanIsValid(iban);
        await CheckIfIbanExistsAsync(iban);
        
        var account = new Account(customerId, accountType, iban);

        return account;
    }
    public async Task<Account>UpdateAccountAsync(Guid accountId,AccountType accountType,string iban)
    {
        iban = CheckIfIbanContainsWhiteSpace(iban);
        await CheckIfAccountExistsAsync(accountId);

        var account = await _accountRepository.FindAsync(accountId);
        
        if(account.Iban != iban)
        {
            await CheckIfIbanExistsAsync(iban);
            CheckIfIbanIsValid(iban);
        }
        
        account = UpdateAccountFields(account, accountType, iban);
        
        return account;
    }

    public async Task<Account> DeleteAccountAsync(Guid accountId)
    {
        await CheckIfAccountExistsAsync(accountId);

        var account = await _accountRepository.FindAsync(accountId);
        
        return account;
    }

    public async Task<Account> GetAccountAsync(Guid accountId)
    {
        await CheckIfAccountExistsAsync(accountId);

        var account = await _accountRepository.FindAsync(accountId);
        
        return account;
    }

    public async Task<List<Account>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.GetListAsync();

        return accounts;
    }

    public async Task<List<Account>> GetAllByCustomerIdAsync(Guid customerId)
    {
        var accounts = await _accountRepository.GetListAsync(x => x.CustomerId == customerId);

        return accounts;
    }

    public async Task CheckIfCustomerExistsAsync(Guid customerId)
    {
        if (await _customerRepository.FirstOrDefaultAsync(x => x.Id == customerId) == null)
        {
            throw new UserFriendlyException(BusinessMessages.CustomerMessages.CustomerNotFound);
        }
    }
    private async Task CheckIfIbanExistsAsync(string iban)
    {
        if (await _accountRepository.FirstOrDefaultAsync(x => x.Iban == iban) != null)
        {
            throw new UserFriendlyException(BusinessMessages.AccountMessages.IbanIsAlreadyInUse);
        }
    }

    public async Task CheckIfAccountExistsAsync(Guid id)
    {
        if (await _accountRepository.FindAsync(id) == null)
        {
            throw new UserFriendlyException(BusinessMessages.AccountMessages.AccountNotFound);
        }
    }

    private static void CheckIfIbanIsValid(string iban)
    {
        if (iban.Length != AccountConstants.IbanLenght - 6)
        {
            throw new UserFriendlyException(BusinessMessages.AccountMessages.IbanIsNotValid);
        }
    }

    private static string CheckIfIbanContainsWhiteSpace(string iban)
    {
        if (iban.Contains(' '))
        {
            iban = RemoveWhiteSpaces(iban);
        }

        return iban;
    }
    
    private static string RemoveWhiteSpaces(string iban)
    {
        iban = string.Join("", iban.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        return iban;
    }

    private static Account UpdateAccountFields(Account account,AccountType accountType, string iban)
    {
        account.AccountType = accountType;
        account.Iban = iban;
        return account;
    }
    
    
}