using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.CardDtos;
using BankApp.Entities;
using BankApp.Managers;
using BankApp.Repositories;
using Volo.Abp.Application.Services;

namespace BankApp.Services;

public class CardAppService : ApplicationService, ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly CardManager _cardManager;
    private readonly AccountManager _accountManager;

    public CardAppService(AccountManager accountManager, CardManager cardManager, ICardRepository cardRepository)
    {
        _accountManager = accountManager;
        _cardManager = cardManager;
        _cardRepository = cardRepository;
    }

    public async Task<CardGetDto> CreateCreditCardAsync(CreditCardCreateDto creditCardCreateDto)
    {
        await _accountManager.CheckIfAccountExistsAsync(creditCardCreateDto.AccountId);

        var card = await _cardManager.CreateCardAsync(creditCardCreateDto.AccountId,creditCardCreateDto.CardNumber,creditCardCreateDto.Balance);
        
        await _cardRepository.InsertAsync(card, true);

        return ObjectMapper.Map<Card, CardGetDto>(card);    
    }

    public async Task<CardGetDto> CreateDebitCardAsync(DebitCardCreateDto debitCardCreateDto)
    {
        await _accountManager.CheckIfAccountExistsAsync(debitCardCreateDto.AccountId);

        var card = await _cardManager.CreateCardAsync(debitCardCreateDto.AccountId,debitCardCreateDto.CardNumber);

        await _cardRepository.InsertAsync(card, true);
        
        return ObjectMapper.Map<Card, CardGetDto>(card);
    }

    public async Task<CardGetDto> UpdateAsync(Guid id, CardUpdateDto cardUpdateDto)
    {
        
        var card = await _cardManager.UpdateCardAsync(id , cardUpdateDto.CardNumber);

        await _cardRepository.UpdateAsync(card);
        
        return ObjectMapper.Map<Card, CardGetDto>(card);
    }

    public async Task<CardGetDto> DeleteAsync(Guid id)
    {
        var card = await _cardManager.DeleteCardAsync(id);

        await _cardRepository.DeleteAsync(id);
        
        return ObjectMapper.Map<Card, CardGetDto>(card);
    }

    public async Task<CardGetDto> GetCardAsync(Guid id)
    {
        var card = await _cardManager.GetCardAsync(id);

        return ObjectMapper.Map<Card, CardGetDto>(card);
    }

    public async Task<List<CardGetDto>> GetAllCardsAsync()
    {
        var cards = await _cardManager.GetAllCardsAsync();
        
        return ObjectMapper.Map<List<Card>, List<CardGetDto>>(cards);
    }

    public async Task<List<CardGetDto>> GetAllByAccountId(Guid accountId)
    {
        await _accountManager.CheckIfAccountExistsAsync(accountId);

        var cards = await _cardManager.GetAllByAccountIdAsync(accountId);
        
        return ObjectMapper.Map<List<Card>, List<CardGetDto>>(cards);
    }
}