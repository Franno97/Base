using Microsoft.Extensions.DependencyInjection;
using Mre.Sb.AuditoriaConf.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Mre.Sb.AuditoriaConf.EntityFrameworkCore
{
    [DependsOn(
       typeof(AuditoriaConfDomainModule),
       typeof(AbpEntityFrameworkCoreModule)
   )]
    public class AuditoriaConfEntityFrameworkModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AuditoriaConfDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true); 

            });
        }
    }

}
