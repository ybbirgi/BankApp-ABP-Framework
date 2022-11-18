using System;
using System.Threading.Tasks;
using BankApp.Constants;
using BankApp.Dtos.CustomerDtos;
using BankApp.Services;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace BankApp;

public sealed class CustomerAppServiceTests : BankAppApplicationTestBase
{
    private readonly ICustomerService _customerService;
    private readonly CustomerCreateDto _customerCreateDto;
    private readonly CustomerUpdateDto _customerUpdateDto;
    private Exception _exception;

    public CustomerAppServiceTests()
    {
        _customerService = GetRequiredService<ICustomerService>();
        _customerCreateDto = new CustomerCreateDto()
        {
            Name = "Yusuf Besim",
            LastName = "Birgi",
            IdentityNumber = "96325874456",
            BirthPlace = "Eskisehir",
            BirthDate = new DateTime(1998,02,16),
            RiskLimit = 10000
        };
        _customerUpdateDto = new CustomerUpdateDto(){
            Name = "Yusuf Besim",
            LastName = "Birgi",
            IdentityNumber = "96325874456",
            BirthPlace = "Eskisehir",
            BirthDate = new DateTime(1998,02,16),
            RiskLimit = 10000
        };

    }

    [Fact]
    public async Task Should_Add_Customer()
    {
        var result = await _customerService.CreateAsync(_customerCreateDto);
        
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("Yusuf Besim");
    }

    [Fact]
    public async Task Should_NOT_Add_Customer_Since_Identity_Number_Exists()
    {
        _customerCreateDto.IdentityNumber = "11111111111";
        
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.CreateAsync(_customerCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberIsInUse);
    }

    [Fact]
    public async Task Should_NOT_Add_Customer_Since_Identity_Number_Is_NOT_Valid()
    {
        _customerCreateDto.IdentityNumber = "123";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.CreateAsync(_customerCreateDto);
        });

        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberMustBe11Digits);

    }

    [Fact]
    public async Task Should_Update_Customer()
    {
        _customerUpdateDto.Name = "Yusuf";
        _customerUpdateDto.LastName = "Besim";
        _customerUpdateDto.IdentityNumber = "99999999999";
        _customerUpdateDto.RiskLimit = 20000;

        var result = await _customerService.UpdateAsync(TestConstants.CustomerId, _customerUpdateDto);
        
        result.Id.ShouldBe(TestConstants.CustomerId);
        result.Name.ShouldBe("Yusuf");
        result.LastName.ShouldBe("Besim");
        result.IdentityNumber.ShouldBe("99999999999");
        result.RiskLimit.ShouldBe(20000);
    }
    [Fact]
    public async Task Should_NOT_Update_Customer_Since_Customer_Not_Found()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.UpdateAsync(Guid.NewGuid(), _customerUpdateDto);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);

    }
    [Fact]
    public async Task Should_NOT_Update_Customer_Since_Identity_Number_NOT_Valid()
    {
        _customerUpdateDto.IdentityNumber = "123123";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.UpdateAsync(TestConstants.CustomerId, _customerUpdateDto);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberMustBe11Digits);
    }
    [Fact]
    public async Task Should_NOT_Update_Customer_Since_Identity_Number_Is_Used()
    {
        _customerUpdateDto.IdentityNumber = "22222222222";
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.UpdateAsync(TestConstants.CustomerId, _customerUpdateDto);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.IdentityNumberIsInUse);
    }
    [Fact]
    public async Task Should_NOT_Update_Customer_Since_Remaining_Limit_NOT_Valid()
    {
        _customerUpdateDto.RiskLimit = 4000;
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.UpdateAsync(TestConstants.CustomerId, _customerUpdateDto);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.InvalidRiskLimit);
    }
    [Fact]
    public async Task Should_Delete_Customer()
    {
        
        var result = await _customerService.DeleteAsync(TestConstants.CustomerId);
        
        result.IsDeleted.ShouldBe(true);
    }
    [Fact]
    public async Task Should_NOT_Delete_Customer_Since_Customer_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.DeleteAsync(Guid.NewGuid());
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);
    }
    [Fact]
    public async Task Should_NOT_Delete_Customer_Since_Customer_Has_Debt()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.DeleteAsync(TestConstants.CustomerId2);
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerHasDebt);
    }
    [Fact]
    public async Task Should_Get_Customer()
    {
        var result = await _customerService.GetCustomerAsync(TestConstants.CustomerId);
        
        result.Name.ShouldBe("Burak");
    }
    [Fact]
    public async Task Should_NOT_Get_Customer_Since_Customer_NOT_Exist()
    {
        _exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        { 
            await _customerService.GetCustomerAsync(Guid.NewGuid());
        });
        
        _exception.Message.ShouldBe(BusinessMessages.CustomerMessages.CustomerNotFound);
    }

    [Fact]
    public async Task Should_Get_All_Customers()
    {
        var result = await _customerService.GetAllCustomersAsync();
        
        result.Count.ShouldBe(2);
    }
}