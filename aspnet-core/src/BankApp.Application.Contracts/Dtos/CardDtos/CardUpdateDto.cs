using System.ComponentModel.DataAnnotations;
using BankApp.Constants.Card;
using Volo.Abp.Validation;

namespace BankApp.Dtos.CardDtos;

public class CardUpdateDto
{
    [Required] [DynamicStringLength(typeof(CardConstants), nameof(CardConstants.CardNumberLenght))]
    public string CardNumber  { get; set; }

    public CardUpdateDto()
    {
        
    }
}