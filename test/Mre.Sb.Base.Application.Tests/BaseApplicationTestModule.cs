using Volo.Abp.Modularity;

namespace Mre.Sb.Base
{
    [DependsOn(
        typeof(BaseApplicationModule),
        typeof(BaseDomainTestModule)
        )]
    public class BaseApplicationTestModule : AbpModule
    {

    }
}