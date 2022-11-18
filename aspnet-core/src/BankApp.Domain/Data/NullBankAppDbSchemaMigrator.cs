using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace BankApp.Data;

/* This is used if database provider does't define
 * IBankAppDbSchemaMigrator implementation.
 */
public class NullBankAppDbSchemaMigrator : IBankAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
