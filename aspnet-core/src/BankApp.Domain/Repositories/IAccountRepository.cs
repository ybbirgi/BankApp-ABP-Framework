using System;
using System.Threading.Tasks;
using BankApp.Entities;
using Volo.Abp.Domain.Repositories;

namespace BankApp.Repositories;

public interface IAccountRepository : IRepository<Account, Guid>
{
}