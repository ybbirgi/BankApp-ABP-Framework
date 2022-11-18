using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.CardDtos;
using Volo.Abp.Application.Services;

namespace BankApp.Services;

public interface ICardService : IApplicationService
{
    Task<CardGetDto> CreateCreditCardAsync(CreditCardCreateDto creditCardCreateDto);

    Task<CardGetDto> CreateDebitCardAsync(DebitCardCreateDto debitCardCreateDto);
    Task<CardGetDto> UpdateAsync(Guid id,CardUpdateDto cardUpdateDto);

    Task<CardGetDto> DeleteAsync(Guid id);

    Task<CardGetDto> GetCardAsync(Guid id);

    Task<List<CardGetDto>> GetAllCardsAsync();

    Task<List<CardGetDto>> GetAllByAccountId(Guid accountId);
}