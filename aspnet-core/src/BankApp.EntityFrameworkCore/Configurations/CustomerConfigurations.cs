using BankApp.Customers;
using BankApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BankApp.Configurations;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", "dbo");
        builder.HasKey(x => x.Id);
        builder.ConfigureByConvention();
        builder.Property(x => x.Name).HasMaxLength(CustomerConstants.NameLenght).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(CustomerConstants.LastNameLenght).IsRequired();
        builder.Property(x => x.IdentityNumber).HasMaxLength(CustomerConstants.IdentityNumberLenght).IsRequired();
        builder.Property(x => x.RiskLimit).HasDefaultValue(CustomerConstants.DefaultRiskLimit);
        builder.Property(x => x.RemainingRiskLimit).HasDefaultValue(CustomerConstants.DefaultRemainingRiskLimit);
        builder.HasMany(x => x.Accounts);
        builder.HasMany(x => x.TransactionHistories);
    }
}