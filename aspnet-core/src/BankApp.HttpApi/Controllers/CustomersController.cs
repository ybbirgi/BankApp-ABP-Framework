using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.CustomerDtos;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

[ApiController]
[Route("Customer")]
public class CustomersController : BankAppController
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<CustomerGetDto> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
    {
        return await _customerService.CreateAsync(customerCreateDto);
    }
    [HttpPut]
    [Route("update")]
    public async Task<CustomerGetDto> UpdateCustomerAsync(Guid id,CustomerUpdateDto customerUpdateDto)
    {
        return await _customerService.UpdateAsync(id,customerUpdateDto);
    }
    [HttpDelete]
    [Route("delete")]
    public async Task<CustomerGetDto> DeleteCustomerAsync(Guid id)
    {
        return await _customerService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<CustomerGetDto> GetCustomerById(Guid id)
    {
        return await _customerService.GetCustomerAsync(id);
    }
    [HttpGet]
    [Route("GetAll")]
    public async Task<List<CustomerGetDto>> GetAllCustomers()
    {
        return await _customerService.GetAllCustomersAsync();
    }
    
    
}