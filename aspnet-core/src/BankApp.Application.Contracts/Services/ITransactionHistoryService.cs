using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Dtos.TransactionHistoryDtos;
using Volo.Abp.Application.Services;

namespace BankApp.Services;

public interface ITransactionHistoryService : IApplicationService
{
    Task<TransactionHistoryGetDto> CreateAsync(TransactionHistoryCreateDto transactionHistoryCreateDto);
    
    Task<TransactionHistoryGetDto> GetTransactionByIdAsync(Guid id);

    Task<List<TransactionHistoryGetDto>> GetAllTransactionsAsync();

    Task<List<TransactionHistoryGetDto>> GetAllTransactionsByCardIdAsync(Guid cardId);
    
    Task<List<TransactionHistoryGetDto>> GetAllTransactionsByCustomerIdAsync(Guid customerId);

}