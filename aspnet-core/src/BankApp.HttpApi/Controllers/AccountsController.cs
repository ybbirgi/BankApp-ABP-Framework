using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.AccountDtos;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

[ApiController]
[Route("Account")]
public class AccountsController : BankAppController
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<AccountGetDto> CreateAccountAsync(AccountCreateDto accountCreateDto)
    {
        return await _accountService.CreateAsync(accountCreateDto);
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<AccountGetDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountUpdateDto)
    {
        return await _accountService.UpdateAsync(id,accountUpdateDto);
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<AccountGetDto> DeleteAccountAsync(Guid id)
    {
        return await _accountService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<AccountGetDto> GetAccountByIdAsync(Guid id)
    {
        return await _accountService.GetAccountAsync(id);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<List<AccountGetDto>> GetAllAccountsAsync()
    {
        return await _accountService.GetAllAccountsAsync();
    }

    [HttpGet]
    [Route("GetAllByCustomerId")]
    public async Task<List<AccountGetDto>> GetAllByCustomerIdAsync(Guid customerId)
    {
        return await _accountService.GetAllByCustomerIdAsync(customerId);
    }
}