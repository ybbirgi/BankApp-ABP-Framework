using System;
using BankApp.Enums;
using Volo.Abp.Application.Dtos;

namespace BankApp.Dtos.CardDtos;

public class CardGetDto : FullAuditedEntityDto<Guid>
{
    public Guid Id { get; set; }
    public Guid AccountId  { get; set; }
    public CardType CardType  { get; set; }
    public string CardNumber  { get; set; }
    public float Balance  { get; set; }
    public float Debt  { get; set; }
}