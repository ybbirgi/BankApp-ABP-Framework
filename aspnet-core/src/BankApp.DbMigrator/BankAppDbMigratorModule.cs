using BankApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace BankApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BankAppEntityFrameworkCoreModule),
    typeof(BankAppApplicationContractsModule)
    )]
public class BankAppDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
