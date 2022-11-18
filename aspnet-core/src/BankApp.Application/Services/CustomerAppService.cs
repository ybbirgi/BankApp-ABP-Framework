using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.CustomerDtos;
using BankApp.Entities;
using BankApp.Managers;
using BankApp.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace BankApp.Services;

public class CustomerAppService : ApplicationService, ICustomerService 
{
    private CustomerManager CustomerManager { get; }
    private readonly ICustomerRepository _customerRepository;

    public CustomerAppService(CustomerManager customerManager, ICustomerRepository customerRepository)
    {
        CustomerManager = customerManager;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerGetDto> CreateAsync(CustomerCreateDto input)
    {
        var customer = await CustomerManager.CreateCustomerAsync(input.Name, input.LastName, input.IdentityNumber,
             input.BirthPlace, input.BirthDate, input.RiskLimit);
        
        await _customerRepository.InsertAsync(customer,true);
        
        return ObjectMapper.Map<Customer, CustomerGetDto>(customer);
    }

    public async Task<CustomerGetDto> UpdateAsync(Guid id,CustomerUpdateDto input)
    {
        var customer = await CustomerManager.UpdateCustomerAsync(id, input.Name, input.LastName, input.IdentityNumber ,
            input.BirthPlace, input.BirthDate, input.RiskLimit);
        
        await _customerRepository.UpdateAsync(customer);
        return ObjectMapper.Map<Customer, CustomerGetDto>(customer);
    }

    public async Task<CustomerGetDto> DeleteAsync(Guid id)
    {
        var customer = await CustomerManager.DeleteCustomerAsync(id);
        
        await _customerRepository.DeleteAsync(customer);
        
        return ObjectMapper.Map<Customer, CustomerGetDto>(customer);
    }

    public async Task<CustomerGetDto> GetCustomerAsync(Guid id)
    {
        var customer = await CustomerManager.GetCustomerAsync(id);

        return ObjectMapper.Map<Customer, CustomerGetDto>(customer);
    }

    public async Task<List<CustomerGetDto>> GetAllCustomersAsync()
    {
        var customerList = await CustomerManager.GetAllCustomersAsync();

        return ObjectMapper.Map<List<Customer>, List<CustomerGetDto>>(customerList);
    }
}