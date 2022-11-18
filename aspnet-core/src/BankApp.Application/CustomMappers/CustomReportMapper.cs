using BankApp.Dtos.TransactionHistoryDtos;
using BankApp.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace BankApp.CustomMappers;

public class CustomReportMapper : IObjectMapper<Card,ReportGetDto> , ITransientDependency
{
    public ReportGetDto Map(Card source)
    {
        var reportGetDto = new ReportGetDto
        {
            AccountId = source.AccountId,
            CardType = source.CardType,
            CardNumber = source.CardNumber,
            Balance = source.Balance,
            Debt = source.Debt
        };
        return reportGetDto;
    }

    public ReportGetDto Map(Card source, ReportGetDto destination)
    {
        throw new System.NotImplementedException();
    }
}