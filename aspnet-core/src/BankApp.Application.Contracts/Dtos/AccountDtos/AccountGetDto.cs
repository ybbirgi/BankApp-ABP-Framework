using System;
using BankApp.Enums;
using Volo.Abp.Application.Dtos;

namespace BankApp.Dtos.AccountDtos;

public class AccountGetDto : FullAuditedEntityDto<Guid>
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public AccountType AccountType { get; set; }
    public string Iban { get; set; }
}