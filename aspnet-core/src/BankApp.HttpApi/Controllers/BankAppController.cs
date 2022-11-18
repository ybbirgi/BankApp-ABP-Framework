using BankApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace BankApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BankAppController : AbpControllerBase
{
    protected BankAppController()
    {
        LocalizationResource = typeof(BankAppResource);
    }
}
