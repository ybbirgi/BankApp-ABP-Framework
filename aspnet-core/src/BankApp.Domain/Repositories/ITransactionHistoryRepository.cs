using System;
using BankApp.Entities;
using Volo.Abp.Domain.Repositories;

namespace BankApp.Repositories;

public interface ITransactionHistoryRepository : IRepository<TransactionHistory,Guid>
{
}