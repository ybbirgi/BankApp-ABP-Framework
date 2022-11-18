using System;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Dtos.TransactionHistoryDtos;
using BankApp.Entities;
using BankApp.Enums;
using BankApp.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BankApp.Services;

public class ReportAppService : ApplicationService , IReportService
{
    private readonly ICardRepository _cardRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;

    public ReportAppService(ICardRepository cardRepository, ITransactionHistoryRepository transactionHistoryRepository)
    {
        _cardRepository = cardRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
    }

    public async Task<ReportGetDto> GetCardReport(Guid cardId)
    {
        var card = await _cardRepository.FirstOrDefaultAsync(x => x.Id == cardId);
        var reportGetDto = ObjectMapper.Map<Card, ReportGetDto>(card);
        
        var transactionListForWithdraws =
            await _transactionHistoryRepository.GetListAsync(x => x.CardId == cardId && x.TransactionDirection == TransactionDirection.Out);
        
        reportGetDto.TotalSpending = transactionListForWithdraws.Sum(x => x.Amount );
        reportGetDto.NumberOfSpendings = transactionListForWithdraws.Count;
        reportGetDto.MaxAmountSpent = transactionListForWithdraws.Max(x => x.Amount);
        reportGetDto.LastAmountSpent = (transactionListForWithdraws.OrderBy(x => x.TransactionDate).First()).Amount;


        return reportGetDto;
    }
}