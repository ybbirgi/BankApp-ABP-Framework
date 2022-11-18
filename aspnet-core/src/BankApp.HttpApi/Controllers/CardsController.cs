using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.CardDtos;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

[ApiController]
[Route("Card")]
public class CardsController : BankAppController
{
    private readonly ICardService _cardService;

    public CardsController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpPost]
    [Route("createCreditCard")]
    public async Task<CardGetDto> CreateCreditCardAsync(CreditCardCreateDto creditCardCreateDto)
    {
        return await _cardService.CreateCreditCardAsync(creditCardCreateDto);
    }
    
    [HttpPost]
    [Route("createDebitCard")]
    public async Task<CardGetDto> CreateDebitCardAsync(DebitCardCreateDto debitCardCreateDto)
    {
        return await _cardService.CreateDebitCardAsync(debitCardCreateDto);
    }

    [HttpPut]
    [Route("update")]
    public async Task<CardGetDto> UpdateAsync(Guid id, CardUpdateDto cardUpdateDto)
    {
        return await _cardService.UpdateAsync(id,cardUpdateDto);
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<CardGetDto> DeleteAsync(Guid id)
    {
        return await _cardService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<CardGetDto> GetCardAsync(Guid id)
    {
        return await _cardService.GetCardAsync(id);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<List<CardGetDto>> GetAllCardsAsync()
    {
        return await _cardService.GetAllCardsAsync();
    }

    [HttpGet]
    [Route("GetByAccountId")]
    public async Task<List<CardGetDto>> GetAllByAccountId(Guid accountId)
    {
        return await _cardService.GetAllByAccountId(accountId);
    }
}