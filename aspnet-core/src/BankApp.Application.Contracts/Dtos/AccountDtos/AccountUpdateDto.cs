using System;
using System.ComponentModel.DataAnnotations;
using BankApp.Customers;
using BankApp.Enums;
using Volo.Abp.Validation;

namespace BankApp.Dtos.AccountDtos;

public class AccountUpdateDto
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    public AccountType AccountType { get; set; }
    [Required]
    [DynamicStringLength(typeof(CustomerConstants), nameof(CustomerConstants.NameLenght))]
    public string Iban { get; set; }
}