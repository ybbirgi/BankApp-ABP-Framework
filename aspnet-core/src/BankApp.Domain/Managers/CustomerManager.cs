using BankApp.Entities;
using BankApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Customers;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace BankApp.Managers
{
    public class CustomerManager : DomainService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer>CreateCustomerAsync(string name, string lastName, string identityNumber, string birthPlace, DateTime birthDate, float riskLimit)
        {
            CheckIfIdentityNumberIsValid(identityNumber);
            await CheckIfIdentityNumberExistsAsync(identityNumber);

            var customer = new Customer(name, lastName, identityNumber, birthPlace, birthDate, riskLimit)
                {
                    RemainingRiskLimit = riskLimit,
                };

            return customer;
        }

        public async Task<Customer>UpdateCustomerAsync(Guid id, string name, string lastName, string identityNumber, string birthPlace, DateTime birthDate, float riskLimit)
        {
            await CheckIfCustomerExistsAsync(id);
            
            var customer = _customerRepository.FindAsync(id).Result;
            if (customer.IdentityNumber != identityNumber)
            {
                CheckIfIdentityNumberIsValid(identityNumber);
                await CheckIfIdentityNumberExistsAsync(identityNumber);
            }
            
            customer = UpdateCustomerFields(customer, name, lastName, identityNumber, birthPlace, birthDate, riskLimit);

            return customer;
        }

        public async Task<Customer>DeleteCustomerAsync(Guid id)
        {
            await CheckIfCustomerExistsAsync(id);

            var customer = await _customerRepository.FindAsync(id);

            CheckIfCustomerHasDebt(customer);

            return customer;
        }
        public async Task<Customer>GetCustomerAsync(Guid id)
        {
            await CheckIfCustomerExistsAsync(id);
            
            var customer = await _customerRepository.FindAsync(id);

            return customer;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetListAsync();

            return customers;
        }
        
        private async Task CheckIfIdentityNumberExistsAsync(string identityNumber)
        {
            if (await _customerRepository.FirstOrDefaultAsync(x => x.IdentityNumber == identityNumber) != null)
            {
                throw new UserFriendlyException(BusinessMessages.CustomerMessages.IdentityNumberIsInUse);
            }
        }
        private static void CheckIfIdentityNumberIsValid(string identityNumber)
        {
            if (identityNumber.Length != CustomerConstants.IdentityNumberLenght)
            {
                throw new UserFriendlyException(BusinessMessages.CustomerMessages.IdentityNumberMustBe11Digits);
            }
        }
        public async Task CheckIfCustomerExistsAsync(Guid id)
        {
            if(await _customerRepository.FindAsync(id) == null)
            {
                throw new UserFriendlyException(BusinessMessages.CustomerMessages.CustomerNotFound);
            }
        }
        private static void CheckIfCustomerHasDebt(Customer customer)
        {
            if (customer.RemainingRiskLimit != customer.RiskLimit)
            {
                throw new UserFriendlyException(BusinessMessages.CustomerMessages.CustomerHasDebt);
            }
        }

        private static void IsRemainingLimitValid(float inputRiskLimit,float remainingRiskLimit)
        {
            if (remainingRiskLimit > inputRiskLimit)
            {
                throw new UserFriendlyException(BusinessMessages.CustomerMessages.InvalidRiskLimit);
            }
        }

        private Customer UpdateCustomerFields(Customer customer, string name, string lastName, string identityNumber, 
                                                                    string birthPlace, DateTime birthDate, float riskLimit)
        {
            IsRemainingLimitValid(riskLimit, customer.RemainingRiskLimit);
            customer.Name = name;
            customer.LastName = lastName;
            customer.IdentityNumber = identityNumber;
            customer.BirthPlace = birthPlace;
            customer.BirthDate = birthDate;
            customer.RemainingRiskLimit = customer.RemainingRiskLimit + (riskLimit - (customer.RiskLimit+customer.RemainingRiskLimit));
            customer.RiskLimit = riskLimit;
            return customer;
        }
        
    }
}
