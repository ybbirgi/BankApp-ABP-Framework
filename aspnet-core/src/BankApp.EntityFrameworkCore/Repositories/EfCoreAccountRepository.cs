using System;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Entities;
using BankApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BankApp.Repositories;

public class EfCoreAccountRepository : EfCoreRepository<BankAppDbContext, Account, Guid>, IAccountRepository
{
    public EfCoreAccountRepository(IDbContextProvider<BankAppDbContext> dbContextProvider) : base(dbContextProvider)
    {
        
    }
}