using System;
using System.Threading.Tasks;
using BankApp.Dtos.TransactionHistoryDtos;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

[ApiController]
[Route("Report")]
public class ReportsController : BankAppController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    [HttpGet]
    [Route("create")]
    public async Task<ReportGetDto> GetCardReport(Guid cardId)
    {
        return await _reportService.GetCardReport(cardId);
    }
}