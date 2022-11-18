using Volo.Abp.Settings;

namespace BankApp.Settings;

public class BankAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(BankAppSettings.MySetting1));
    }
}
