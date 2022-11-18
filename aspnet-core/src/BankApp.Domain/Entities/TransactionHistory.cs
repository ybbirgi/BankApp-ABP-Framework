using System;
using BankApp.Enums;
using Volo.Abp.Domain.Entities;

namespace BankApp.Entities;

public class TransactionHistory : AggregateRoot<Guid>
{
    public Guid CustomerId { get; set; }
    public Guid CardId { get; set; }
    public float Amount { get; set; }
    public TransactionDirection TransactionDirection { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Definition { get; set; }
    public DateTime TransactionDate { get; set; }

    public TransactionHistory(Guid customerId,Guid cardId,float amount,TransactionDirection transactionDirection,TransactionType transactionType,string definition)
    {
        CustomerId = customerId;
        CardId = cardId;
        Amount = amount;
        TransactionDirection = transactionDirection;
        TransactionType = transactionType;
        Definition = definition;
    }
    

    public void SetId(Guid id)
    {
        this.Id = id;
    }
    public virtual Customer Customer { get; set; }
    public virtual Card Card { get; set; }

    public TransactionHistory() { }
}