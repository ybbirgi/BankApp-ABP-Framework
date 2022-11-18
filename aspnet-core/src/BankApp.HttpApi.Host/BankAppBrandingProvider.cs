using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace BankApp;

[Dependency(ReplaceServices = true)]
public class BankAppBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BankApp";
}
