using BankApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BankApp.Permissions;

public class BankAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BankAppPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(BankAppPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BankAppResource>(name);
    }
}
