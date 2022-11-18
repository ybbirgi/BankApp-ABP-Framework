using System;
using System.Threading.Tasks;
using BankApp.Entities;
using Volo.Abp.Domain.Repositories;

namespace BankApp.Repositories;

public interface ICustomerRepository: IRepository<Customer,Guid>
{
}