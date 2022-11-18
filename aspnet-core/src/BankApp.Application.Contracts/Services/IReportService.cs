using System;
using System.Threading.Tasks;
using BankApp.Dtos.TransactionHistoryDtos;
using Volo.Abp.Application.Services;

namespace BankApp.Services;

public interface IReportService : IApplicationService
{
    Task<ReportGetDto> GetCardReport(Guid cardId);
}