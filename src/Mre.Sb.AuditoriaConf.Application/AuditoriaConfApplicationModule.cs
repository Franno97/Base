using Microsoft.Extensions.DependencyInjection;
using Mre.Sb.AuditoriaConf.Domain;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Mre.Sb.AuditoriaConf.Application
{
    [DependsOn(
        typeof(AuditoriaConfDomainModule),
        typeof(AuditoriaConfApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class AuditoriaConfApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AuditoriaConfApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AuditoriaConfApplicationModule>(validate: true);
            });

         
        }

    }
}
