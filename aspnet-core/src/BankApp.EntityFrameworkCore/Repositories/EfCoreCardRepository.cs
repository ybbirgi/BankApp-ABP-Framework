using System;
using BankApp.Entities;
using BankApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BankApp.Repositories;

public class EfCoreCardRepository : EfCoreRepository<BankAppDbContext, Card, Guid>, ICardRepository
{
    public EfCoreCardRepository(IDbContextProvider<BankAppDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}