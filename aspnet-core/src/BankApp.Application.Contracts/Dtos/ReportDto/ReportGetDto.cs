using System;
using BankApp.Enums;

namespace BankApp.Dtos.TransactionHistoryDtos;

public class ReportGetDto
{
    public Guid AccountId { get; set; }
    public CardType CardType { get; set; }
    public string CardNumber { get; set; }
    public float Balance { get; set; }
    public float Debt { get; set; }
    public float? TotalSpending { get; set; } = 0;
    public int? NumberOfSpendings { get; set; } = 0;
    public float? MaxAmountSpent { get; set; } = 0;
    public float? LastAmountSpent { get; set; } = 0;

    public ReportGetDto()
    {
    }
}