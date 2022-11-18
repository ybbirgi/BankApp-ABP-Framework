using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace BankApp.Entities;

public class Customer : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string BirthPlace { get; set; }
    public DateTime BirthDate { get; set; }
    public float RiskLimit { get; set; }
    public float RemainingRiskLimit { get; set; }

    public Customer() { }

    public Customer(string name, string lastName, string identityNumber, string birthPlace
        , DateTime birthDate, float riskLimit = 10000)
    {
        this.Name = name;
        this.LastName = lastName;
        this.IdentityNumber = identityNumber;
        this.BirthPlace = birthPlace;
        this.BirthDate = birthDate;
        this.RiskLimit = riskLimit;
        this.RemainingRiskLimit = this.RiskLimit;
    }

    public void SetId(Guid id)
    {
        this.Id = id;
    }
    public virtual ICollection<Account> Accounts { get; set; }
    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; }
    
}