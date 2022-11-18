using System.Threading.Tasks;

namespace BankApp.Data;

public interface IBankAppDbSchemaMigrator
{
    Task MigrateAsync();
}
