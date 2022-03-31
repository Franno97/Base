using Mre.Sb.Base.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Mre.Sb.Base
{
    [DependsOn(
        typeof(BaseEntityFrameworkCoreTestModule)
        )]
    public class BaseDomainTestModule : AbpModule
    {

    }
}