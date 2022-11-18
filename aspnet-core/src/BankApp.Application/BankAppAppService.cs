using System;
using System.Collections.Generic;
using System.Text;
using BankApp.Localization;
using Volo.Abp.Application.Services;

namespace BankApp;

/* Inherit your application services from this class.
 */
public abstract class BankAppAppService : ApplicationService
{
    protected BankAppAppService()
    {
        LocalizationResource = typeof(BankAppResource);
    }
}
