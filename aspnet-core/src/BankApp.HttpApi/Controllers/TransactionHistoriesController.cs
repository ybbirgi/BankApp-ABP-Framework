using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.TransactionHistoryDtos;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

[ApiController]
[Route("TransactionHistory")]
public class TransactionHistoriesController : BankAppController
{
    public readonly ITransactionHistoryService _TransactionHistoryService;

    public TransactionHistoriesController(ITransactionHistoryService transactionHistoryService)
    {
        _TransactionHistoryService = transactionHistoryService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<TransactionHistoryGetDto> CreateAsync(TransactionHistoryCreateDto transactionHistoryCreateDto)
    {
        return await _TransactionHistoryService.CreateAsync(transactionHistoryCreateDto);
    }


    [HttpGet]
    [Route("GetById")]
    public async Task<TransactionHistoryGetDto> GetTransactionByIdAsync(Guid id)
    {
        return await _TransactionHistoryService.GetTransactionByIdAsync(id);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<List<TransactionHistoryGetDto>> GetAllTransactionsAsync()
    {
        return await _TransactionHistoryService.GetAllTransactionsAsync();
    }

    [HttpGet]
    [Route("GetAllByCardId")]
    public async Task<List<TransactionHistoryGetDto>> GetAllTransactionsByCardIdAsync(Guid cardId)
    {
        return await _TransactionHistoryService.GetAllTransactionsByCardIdAsync(cardId);
    }

    [HttpGet]
    [Route("GetAllByCustomerId")]
    public async Task<List<TransactionHistoryGetDto>> GetAllTransactionsByCustomerIdAsync(Guid customerId)
    {
        return await _TransactionHistoryService.GetAllTransactionsByCustomerIdAsync(customerId);
    }
}
