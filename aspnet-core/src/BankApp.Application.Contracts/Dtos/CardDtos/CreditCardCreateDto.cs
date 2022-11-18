using System;
using System.ComponentModel.DataAnnotations;
using BankApp.Constants.Card;
using BankApp.Enums;
using Volo.Abp.Validation;

namespace BankApp.Dtos.CardDtos;

public class CreditCardCreateDto
{
    [Required] 
    public Guid AccountId  { get; set; }

    [Required] [DynamicStringLength(typeof(CardConstants), nameof(CardConstants.CardNumberLenght))]
    public string CardNumber  { get; set; }

    [Required] public float Balance  { get; set; }

    public CreditCardCreateDto()
    {
    }
}