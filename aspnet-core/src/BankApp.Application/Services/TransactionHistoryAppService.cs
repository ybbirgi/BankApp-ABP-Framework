using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.TransactionHistoryDtos;
using BankApp.Entities;
using BankApp.Managers;
using BankApp.Repositories;
using Volo.Abp.Application.Services;

namespace BankApp.Services;

public class TransactionHistoryAppService : ApplicationService, ITransactionHistoryService
{
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly TransactionHistoryManager _transactionHistoryManager;
    private readonly CardManager _cardManager;

    public TransactionHistoryAppService(ITransactionHistoryRepository transactionHistoryRepository, TransactionHistoryManager transactionHistoryManager, CardManager cardManager)
    {
        _transactionHistoryRepository = transactionHistoryRepository;
        _transactionHistoryManager = transactionHistoryManager;
        _cardManager = cardManager;
    }

    public async Task<TransactionHistoryGetDto> CreateAsync(TransactionHistoryCreateDto transactionHistoryCreateDto)
    {
        var transactionHistory = await _transactionHistoryManager.CreateTransactionHistoryAsync(transactionHistoryCreateDto.CardId,transactionHistoryCreateDto.Amount,
            transactionHistoryCreateDto.TransactionDirection,transactionHistoryCreateDto.TransactionType,transactionHistoryCreateDto.Definition);

        await _transactionHistoryRepository.InsertAsync(transactionHistory);

        return ObjectMapper.Map<TransactionHistory, TransactionHistoryGetDto>(transactionHistory);
    }

    public async Task<TransactionHistoryGetDto> GetTransactionByIdAsync(Guid id)
    {
        var transactionHistory = await _transactionHistoryManager.GetTransactionByIdAsync(id);

        return ObjectMapper.Map<TransactionHistory, TransactionHistoryGetDto>(transactionHistory);
    }

    public async Task<List<TransactionHistoryGetDto>> GetAllTransactionsAsync()
    {
        var transactionHistories = await _transactionHistoryManager.GetAllTransactionsAsync();

        return ObjectMapper.Map<List<TransactionHistory>, List<TransactionHistoryGetDto>>(transactionHistories);
    }

    public async Task<List<TransactionHistoryGetDto>> GetAllTransactionsByCardIdAsync(Guid cardId)
    {
        var transactionHistories = await _transactionHistoryManager.GetAllTransactionsByCardIdAsync(cardId);

        return ObjectMapper.Map<List<TransactionHistory>, List<TransactionHistoryGetDto>>(transactionHistories);
    }

    public async Task<List<TransactionHistoryGetDto>> GetAllTransactionsByCustomerIdAsync(Guid customerId)
    {
        var transactionHistories = await _transactionHistoryManager.GetAllTransactionsByCustomerIdAsync(customerId);

        return ObjectMapper.Map<List<TransactionHistory>, List<TransactionHistoryGetDto>>(transactionHistories);
    }
}