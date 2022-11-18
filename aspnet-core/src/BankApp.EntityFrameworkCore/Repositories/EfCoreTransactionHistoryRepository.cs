using System;
using BankApp.Entities;
using BankApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BankApp.Repositories;

public class EfCoreTransactionHistoryRepository : EfCoreRepository<BankAppDbContext, TransactionHistory, Guid>, ITransactionHistoryRepository
{
    public EfCoreTransactionHistoryRepository(IDbContextProvider<BankAppDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}