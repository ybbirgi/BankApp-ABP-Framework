using BankApp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace BankApp;

[DependsOn(
    typeof(BankAppEntityFrameworkCoreTestModule)
    )]
public class BankAppDomainTestModule : AbpModule
{

}
