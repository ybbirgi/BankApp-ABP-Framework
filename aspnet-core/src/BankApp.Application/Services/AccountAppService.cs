using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.AccountDtos;
using BankApp.Entities;
using BankApp.Managers;
using BankApp.Repositories;
using Volo.Abp.Application.Services;

namespace BankApp.Services;

public class AccountAppService : ApplicationService, IAccountService
{
    private readonly AccountManager _accountManager;
    private readonly CustomerManager _customerManager;
    private readonly IAccountRepository _accountRepository;

    public AccountAppService(AccountManager accountManager,IAccountRepository accountRepository,CustomerManager customerManager)
    {
        _accountManager = accountManager;
        _accountRepository = accountRepository;
        _customerManager = customerManager;
    }
    
    public async Task<AccountGetDto> CreateAsync(AccountCreateDto accountCreateDto)
    {
        await _customerManager.CheckIfCustomerExistsAsync(accountCreateDto.CustomerId);

        var account = await _accountManager.CreateAccountAsync(accountCreateDto.CustomerId, accountCreateDto.AccountType,
            accountCreateDto.Iban);

        await _accountRepository.InsertAsync(account);
        
        return ObjectMapper.Map<Account, AccountGetDto>(account);
    }

    public async Task<AccountGetDto> UpdateAsync(Guid id, AccountUpdateDto accountUpdateDto)
    {
        await _customerManager.CheckIfCustomerExistsAsync(accountUpdateDto.CustomerId);

        var account = await _accountManager.UpdateAccountAsync(id, accountUpdateDto.AccountType, accountUpdateDto.Iban);

        await _accountRepository.UpdateAsync(account);

        return ObjectMapper.Map<Account, AccountGetDto>(account);
    }

    public async Task<AccountGetDto> DeleteAsync(Guid id)
    {
        var account = await _accountManager.DeleteAccountAsync(id);

        await _accountRepository.DeleteAsync(id);
        
        return ObjectMapper.Map<Account, AccountGetDto>(account);
    }

    public async Task<AccountGetDto> GetAccountAsync(Guid id)
    {
        var account = await _accountManager.GetAccountAsync(id);
        
        return ObjectMapper.Map<Account, AccountGetDto>(account);
    }

    public async Task<List<AccountGetDto>> GetAllAccountsAsync()
    {
        var accounts = await _accountManager.GetAllAccountsAsync();

        return ObjectMapper.Map<List<Account>, List<AccountGetDto>>(accounts);
    }

    public async Task<List<AccountGetDto>> GetAllByCustomerIdAsync(Guid customerId)
    {
        await _customerManager.CheckIfCustomerExistsAsync(customerId);

        var accounts = await _accountManager.GetAllByCustomerIdAsync(customerId);
        
        return ObjectMapper.Map<List<Account>, List<AccountGetDto>>(accounts);
    }
}