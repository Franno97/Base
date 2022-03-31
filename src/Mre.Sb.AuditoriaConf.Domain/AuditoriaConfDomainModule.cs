using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Mre.Sb.AuditoriaConf.Domain
{

    [DependsOn(
        typeof(AuditoriaConfDomainSharedModule)
        )]
    public class AuditoriaConfDomainModule : AbpModule
    {
        
    }
}
