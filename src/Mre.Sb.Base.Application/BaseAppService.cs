using System;
using System.Collections.Generic;
using System.Text;
using Mre.Sb.Base.Localization;
using Volo.Abp.Application.Services;

namespace Mre.Sb.Base
{
    /* Inherit your application services from this class.
     */
    public abstract class BaseAppService : ApplicationService
    {
        protected BaseAppService()
        {
            LocalizationResource = typeof(BaseResource);
        }
    }
}
