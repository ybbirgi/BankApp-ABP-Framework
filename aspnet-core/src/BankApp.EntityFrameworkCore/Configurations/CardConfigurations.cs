using BankApp.Constants.Card;
using BankApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BankApp.Configurations;

public class CardConfigurations: IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards", "dbo");
        builder.HasKey(x => x.Id);
        builder.ConfigureByConvention();
        builder.Property(x => x.CardNumber).HasMaxLength(CardConstants.CardNumberLenght).IsRequired();
        builder.Property(x => x.Debt).HasDefaultValue(CardConstants.DefaultDebt);
        builder.HasOne(x => x.Account).WithMany(b => b.Cards).HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.TransactionHistories);
    }
}