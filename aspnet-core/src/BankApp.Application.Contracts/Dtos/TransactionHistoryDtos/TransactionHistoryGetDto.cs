using System;
using BankApp.Enums;

namespace BankApp.Dtos.TransactionHistoryDtos;

public class TransactionHistoryGetDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CardId  { get; set; }
    public float Amount  { get; set; }
    public TransactionDirection TransactionDirection  { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Definition { get; set; }
    public DateTime TransactionDate { get; set; }
}