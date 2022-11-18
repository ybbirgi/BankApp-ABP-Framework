using System;
using System.Collections.Generic;
using System.Dynamic;
using BankApp.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace BankApp.Entities;

public class Account : FullAuditedAggregateRoot<Guid>
{
    public Guid CustomerId { get; set; }
    public AccountType AccountType { get; set; }
    public string Iban { get; set; }

    public Account() { }

    public Account(Guid customerId,AccountType accountType,string iban)
    {
        CustomerId = customerId;
        AccountType = accountType;
        Iban = iban;
    }
    public virtual Customer Customer { get; set; }
    public virtual ICollection<Card> Cards { get; set; }

    public void SetId(Guid id)
    {
        this.Id = id;
    }
}