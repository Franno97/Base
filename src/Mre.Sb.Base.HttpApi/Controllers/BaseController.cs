using Mre.Sb.Base.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Mre.Sb.Base.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class BaseController : AbpController
    {
        protected BaseController()
        {
            LocalizationResource = typeof(BaseResource);
        }
    }
}