using System;
using Volo.Abp.Application.Dtos;

namespace BankApp.Dtos.CustomerDtos;

public class CustomerGetDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string BirthPlace { get; set; }
    public DateTime BirthDate { get; set; }
    public float RiskLimit { get; set; }
    public float RemainingRiskLimit { get; set; }
}