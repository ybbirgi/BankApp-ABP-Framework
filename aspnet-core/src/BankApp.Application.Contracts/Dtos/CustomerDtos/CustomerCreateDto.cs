using System;
using System.ComponentModel.DataAnnotations;
using BankApp.Customers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace BankApp.Dtos.CustomerDtos;

public class CustomerCreateDto : EntityDto
{
    [Required]
    [DynamicStringLength(typeof(CustomerConstants), nameof(CustomerConstants.NameLenght))]
    public string Name { get; set; }
    [Required]
    [DynamicStringLength(typeof(CustomerConstants), nameof(CustomerConstants.LastNameLenght))]
    public string LastName { get; set; }
    [Required]
    [DynamicStringLength(typeof(CustomerConstants), nameof(CustomerConstants.IdentityNumberLenght))]
    public string IdentityNumber { get; set; }
    [Required]
    public string BirthPlace { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    public float RiskLimit { get; set; } = 10000;

    public CustomerCreateDto()
    {
        
    }
}