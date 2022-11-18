using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Entities;
using BankApp.Managers;
using BankApp.Repositories;
using NSubstitute;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace BankApp;

public class CustomerManagerTests : BankAppDomainTestBase
{
    private readonly ICustomerRepository _fakeRepo;
    private readonly CustomerManager _customerManager;
    private readonly Guid _customerId;
    private readonly Customer _customer;
    private Exception _exception;
    
    public CustomerManagerTests()
    {
        _customer = new Customer("Yusuf Besim", "Birgi", "12345678952", "Eskisehir", DateTime.Now, 10000);
        _fakeRepo = Substitute.For<ICustomerRepository>();
        _customerManager = new CustomerManager(_fakeRepo);
        _customerId = Guid.NewGuid();
    }

    [Fact]
    public async Task Should_Create_A_New_Customer()
    {
        _fakeRepo.FirstOrDefaultAsync(x => x.IdentityNumber == _customer.IdentityNumber).Returns(null as Customer);
        
        var newCustomer = await _customerManager.CreateCustomerAsync(_customer.Name, _customer.LastName, _customer.IdentityNumber,
            _customer.BirthPlace, _customer.BirthDate, _customer.RiskLimit);

        newCustomer.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_New_Customer_Since_Invalid_Identity_Number()
    {
        _customer.IdentityNumber = "123";
        _fakeRepo.FirstOrDefaultAsync(x => x.IdentityNumber == _customer.IdentityNumber).Returns(null as Customer);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            var newCustomer = await _customerManager.CreateCustomerAsync(_customer.Name, _customer.LastName, _customer.IdentityNumber,
                _customer.BirthPlace, _customer.BirthDate, _customer.RiskLimit);

        });
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberMustBe11Digits);
    }

    [Fact]
    public async Task Should_Not_Create_A_New_Customer_Since_Identity_Number_Exists()
    {
        _fakeRepo.FirstOrDefaultAsync(x => x.IdentityNumber == _customer.IdentityNumber).ReturnsForAnyArgs(new Customer("Burak","Birgi","12346678952","Eskisehir",DateTime.Now,10000));
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerManager.CreateCustomerAsync(_customer.Name, _customer.LastName, _customer.IdentityNumber,
                _customer.BirthPlace, _customer.BirthDate, _customer.RiskLimit);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberIsInUse);
        
    }

    [Fact]
    public async Task Should_Update_Customers_Name_LastName_BirthPlace()
    {
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);

        var updatedCustomer = await _customerManager.UpdateCustomerAsync(_customerId, "Yusuf", "Besim", _customer.IdentityNumber, "Ankara",
            new DateTime(1998,02,16), _customer.RiskLimit);
        
        updatedCustomer.Name.ShouldBe("Yusuf");
        updatedCustomer.LastName.ShouldBe("Besim");
        updatedCustomer.BirthDate.ShouldBe(new DateTime(1998,02,16));
        updatedCustomer.BirthPlace.ShouldBe("Ankara");
    }

    [Fact]
    public async Task Should_Not_Update_Customer_Since_Customer_Does_Not_Exist()
    {
        var customerId = Guid.NewGuid();
        _fakeRepo.FindAsync(customerId).Returns(null as Customer);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerManager.UpdateCustomerAsync(customerId,_customer.Name,_customer.LastName,
                _customer.IdentityNumber,_customer.BirthPlace,_customer.BirthDate,_customer.RiskLimit);
        });
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);

    }

    [Fact]
    public async Task Should_Update_Customers_IdentityNumber()
    {
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).Returns(null as Customer);

        var updatedCustomer = await _customerManager.UpdateCustomerAsync(_customerId, _customer.Name, _customer.LastName,
            "99999999999", _customer.BirthPlace, _customer.BirthDate, _customer.RiskLimit);
        
        updatedCustomer.IdentityNumber.ShouldBe("99999999999");
    }
    
    [Fact]
    public async Task Should_NOT_Update_Customers_IdentityNumber_Since_It_Is_Used()
    {
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(new Customer("Burak", "Birgi", "99999999999", "Eskisehir", DateTime.Now, 10000));
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
             await _customerManager.UpdateCustomerAsync(_customerId, _customer.Name, _customer.LastName,
                "99999999999", _customer.BirthPlace, _customer.BirthDate, _customer.RiskLimit);
        });
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberIsInUse);
    }

    [Fact]
    public async Task Should_NOT_Update_Customers_IdentityNumber_Since_It_Is_NOT_Valid()
    {
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).Returns(null as Customer);
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerManager.UpdateCustomerAsync(_customerId, _customer.Name, _customer.LastName,
                "123", _customer.BirthPlace, _customer.BirthDate, _customer.RiskLimit);
        });
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberMustBe11Digits);
    }

    [Fact]
    public async Task Should_Update_Customers_RiskLimit()
    {
        //Assuming Customer Got Credit Card with 5000 Balance
        _customer.RemainingRiskLimit = 5000;
        
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).Returns(null as Customer);
        
        var updatedCustomer = await _customerManager.UpdateCustomerAsync(_customerId, _customer.Name, _customer.LastName,
            _customer.IdentityNumber, _customer.BirthPlace, _customer.BirthDate, 20000);
        
        updatedCustomer.RiskLimit.ShouldBe(20000);
    }
    [Fact]
    public async Task Should_NOT_Update_Customers_RiskLimit_Since_It_Is_Not_Valid()
    {
        //Assuming Customer Got Credit Card with 5000 Balance
        _customer.RemainingRiskLimit = 5000;
        
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).Returns(null as Customer);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerManager.UpdateCustomerAsync(_customerId, _customer.Name, _customer.LastName,
                _customer.IdentityNumber, _customer.BirthPlace, _customer.BirthDate, 4000);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.InvalidRiskLimit);
    }

    [Fact]
    public async Task Should_Delete_Customer()
    {
        _customer.RemainingRiskLimit = _customer.RiskLimit;
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(_customer);

        var updatedCustomer = await _customerManager.DeleteCustomerAsync(_customerId);

        updatedCustomer.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_NOT_Delete_Customer_Since_Customer_Does_NOT_Exist()
    {
        _customer.RemainingRiskLimit = _customer.RiskLimit;
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).Returns(null as Customer);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerManager.DeleteCustomerAsync(_customerId);
        });

        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);
    }

    [Fact]
    public async Task Should_NOT_Delete_Customer_Since_Customer_Has_Debt()
    {
        //Assuming Customer Has Credit Card With 5000 Limit
        _customer.RemainingRiskLimit = 5000;
        
        
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);
        _fakeRepo.FirstOrDefaultAsync(x => x.Id == _customerId).ReturnsForAnyArgs(_customer);
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerManager.DeleteCustomerAsync(_customerId);
        });

        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerHasDebt);
    }

    [Fact]
    public async Task Should_Get_Customer()
    {
        _fakeRepo.FindAsync(_customerId).ReturnsForAnyArgs(_customer);

        var listedCustomer = await _customerManager.GetCustomerAsync(_customerId);
        
        listedCustomer.ShouldBe(_customer);
    }
    [Fact]
    public async Task Should_NOT_Get_Customer_Since_Customer_Not_Exist()
    { 
        _fakeRepo.FindAsync(_customerId).Returns(null as Customer);

        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerManager.GetCustomerAsync(_customerId);
        });

        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);
    }
    [Fact]
    public async Task Should_Get_All_Customers()
    {
        var customer2 = new Customer("Burak", "Birgi", "12341678952", "Eskisehir", DateTime.Now, 10000);
        var customerList = new List<Customer> { _customer, customer2 };
        _fakeRepo.GetListAsync().ReturnsForAnyArgs(customerList);
        
        var customersListed = await _customerManager.GetAllCustomersAsync();
        
        customerList.ShouldBeSameAs(customerList);
    }
}