using AutoMapper;
using BankApp.Dtos.AccountDtos;
using BankApp.Dtos.CardDtos;
using BankApp.Dtos.CustomerDtos;
using BankApp.Dtos.TransactionHistoryDtos;
using BankApp.Entities;
using Volo.Abp.AutoMapper;

namespace BankApp;

public class BankAppApplicationAutoMapperProfile : Profile
{
    public BankAppApplicationAutoMapperProfile()
    {
        CreateMap<Customer, CustomerGetDto>();
        CreateMap<CustomerCreateDto, Customer>(MemberList.Source).IgnoreFullAuditedObjectProperties();
        CreateMap<CustomerUpdateDto, Customer>(MemberList.Source).IgnoreFullAuditedObjectProperties();
        CreateMap<Account, AccountGetDto>();
        CreateMap<AccountCreateDto, Account>(MemberList.Source).IgnoreFullAuditedObjectProperties();
        CreateMap<AccountUpdateDto, Account>(MemberList.Source).IgnoreFullAuditedObjectProperties();
        CreateMap<Card, CardGetDto>();
        CreateMap<CreditCardCreateDto, Card>(MemberList.Source).IgnoreFullAuditedObjectProperties();
        CreateMap<DebitCardCreateDto, Card>(MemberList.Source).IgnoreFullAuditedObjectProperties();
        CreateMap<CardUpdateDto, Card>(MemberList.Source).IgnoreFullAuditedObjectProperties();
        CreateMap<TransactionHistory, TransactionHistoryGetDto>();
        CreateMap<TransactionHistoryCreateDto, TransactionHistory>();
        CreateMap<Card, ReportGetDto>();
    }
}
