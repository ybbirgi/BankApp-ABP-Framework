using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.CustomerDtos;
using Volo.Abp.Application.Services;

namespace BankApp.Services;

public interface ICustomerService : IApplicationService
{
     Task<CustomerGetDto> CreateAsync(CustomerCreateDto customerCreateDto);

     Task<CustomerGetDto> UpdateAsync(Guid id,CustomerUpdateDto customerUpdateDto);

     Task<CustomerGetDto> DeleteAsync(Guid id);

     Task<CustomerGetDto> GetCustomerAsync(Guid id);

     Task<List<CustomerGetDto>> GetAllCustomersAsync();
}