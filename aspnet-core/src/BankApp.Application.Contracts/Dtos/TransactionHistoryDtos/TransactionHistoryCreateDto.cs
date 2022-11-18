using System;
using System.ComponentModel.DataAnnotations;
using BankApp.Constants;
using BankApp.Enums;
using Volo.Abp.Validation;

namespace BankApp.Dtos.TransactionHistoryDtos;

public class TransactionHistoryCreateDto
{
    [Required] 
    public Guid CardId  { get; set; }
    [Required]
    public float Amount  { get; set; }
    [Required]
    public TransactionDirection TransactionDirection  { get; set; }
    [Required]
    public TransactionType TransactionType  { get; set; }
    [Required]
    [DynamicStringLength(typeof(TransactionHistoryConstants), nameof(TransactionHistoryConstants.DefinitionLenght))]
    public string Definition  { get; set; }

    public TransactionHistoryCreateDto()
    {
    }
}