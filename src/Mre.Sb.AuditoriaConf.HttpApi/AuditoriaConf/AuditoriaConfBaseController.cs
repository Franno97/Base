using Mre.Sb.AuditoriaConf.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public abstract class AuditoriaConfBaseController : AbpController
    {
        protected AuditoriaConfBaseController()
        {
            LocalizationResource = typeof(AuditoriaConfResource);
        }
    }
     
}
