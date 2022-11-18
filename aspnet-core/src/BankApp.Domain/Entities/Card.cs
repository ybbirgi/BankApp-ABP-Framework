using System;
using System.Collections.Generic;
using BankApp.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace BankApp.Entities;

public class Card : FullAuditedAggregateRoot<Guid>
{
    public Guid AccountId { get; set; }
    public CardType CardType { get; set; }
    public string CardNumber { get; set; }
    public float Balance { get; set; }
    public float Debt { get; set; }

    public Card() { }

    public Card(Guid accountId,CardType cardType,string cardNumber)
    {
        AccountId = accountId;
        CardType = cardType;
        CardNumber = cardNumber;
    }
    public virtual Account Account { get; set; }
    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; }

    public void SetId(Guid id)
    {
        this.Id = id;
    }
}