using Mre.Sb.Base.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Mre.Sb.Base.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BaseEntityFrameworkCoreModule),
        typeof(BaseApplicationContractsModule)
        )]
    public class BaseDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
