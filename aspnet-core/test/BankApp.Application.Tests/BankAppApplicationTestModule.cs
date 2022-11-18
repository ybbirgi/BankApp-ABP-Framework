using Volo.Abp.Modularity;

namespace BankApp;

[DependsOn(
    typeof(BankAppApplicationModule),
    typeof(BankAppDomainTestModule)
    )]
public class BankAppApplicationTestModule : AbpModule
{

}
