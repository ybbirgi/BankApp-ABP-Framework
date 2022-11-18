using BankApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BankApp.Configurations;

public class TransactionHistoryConfigurations: IEntityTypeConfiguration<TransactionHistory>
{
    public void Configure(EntityTypeBuilder<TransactionHistory> builder)
    {
        builder.ToTable("TransactionHistory", "dbo");
        builder.HasKey(x => x.Id);
        builder.ConfigureByConvention();
        builder.HasOne(x => x.Customer).WithMany(b => b.TransactionHistories).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Card).WithMany(b => b.TransactionHistories).HasForeignKey(x => x.CardId).OnDelete(DeleteBehavior.Restrict);
    }

}