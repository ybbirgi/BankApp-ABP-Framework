using System;
using System.ComponentModel.DataAnnotations;
using BankApp.Constants;
using BankApp.Customers;
using BankApp.Enums;
using Volo.Abp.Validation;

namespace BankApp.Dtos.AccountDtos;

public class AccountCreateDto
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    public AccountType AccountType { get; set; }
    [Required]
    [DynamicStringLength(typeof(AccountConstants), nameof(AccountConstants.IbanLenght))]
    public string Iban { get; set; }

    public AccountCreateDto()
    {
        
    }
}