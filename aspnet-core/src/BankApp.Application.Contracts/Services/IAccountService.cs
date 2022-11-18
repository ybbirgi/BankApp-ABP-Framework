using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.AccountDtos;
using Volo.Abp.Application.Services;

namespace BankApp.Services;


public interface IAccountService : IApplicationService
{
    Task<AccountGetDto> CreateAsync(AccountCreateDto accountCreateDto);

    Task<AccountGetDto> UpdateAsync(Guid id,AccountUpdateDto accountUpdateDto);

    Task<AccountGetDto> DeleteAsync(Guid id);

    Task<AccountGetDto> GetAccountAsync(Guid id);

    Task<List<AccountGetDto>> GetAllAccountsAsync();

    Task<List<AccountGetDto>> GetAllByCustomerIdAsync(Guid customerId);
}