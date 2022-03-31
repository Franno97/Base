using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Mre.Sb.Base
{
    [Dependency(ReplaceServices = true)]
    public class BaseBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Cancillería del Ecuador";
    }
}
