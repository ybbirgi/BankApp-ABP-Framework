using BankApp.Constants;
using BankApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BankApp.Configurations;

public class AccountConfigurations: IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts", "dbo");
        builder.HasKey(x => x.Id);
        builder.ConfigureByConvention();
        builder.Property(x => x.Iban).HasMaxLength(AccountConstants.IbanLenght).IsRequired();
        builder.HasOne(x => x.Customer).WithMany(b => b.Accounts).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Cards);
    }
}