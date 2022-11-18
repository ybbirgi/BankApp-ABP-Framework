using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using BankApp.Entities;
using BankApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BankApp.Repositories;

public class EfCoreCustomerRepository : EfCoreRepository<BankAppDbContext, Customer, Guid>, ICustomerRepository
{
    public EfCoreCustomerRepository(IDbContextProvider<BankAppDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}