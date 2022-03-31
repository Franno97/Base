using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mre.Sb.AuditoriaConf;
using Mre.Sb.Base.MultiTenancy;
using Mre.Sb.Cita;
using Mre.Sb.RegistroPersona;
using Mre.Sb.UnidadAdministrativa;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;


namespace Mre.Sb.Base
{
    [DependsOn(
        typeof(BaseDomainSharedModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpEmailingModule)
    )]

    //Localization other microservices/modules
    [DependsOn(
        typeof(AuditoriaConfDomainSharedModule)
    )]
    [DependsOn(
        typeof(AdministrativeUnitDomainSharedModule) 
    )]

    [DependsOn(
        typeof(PersonRegistrationDomainSharedModule)
    )]

    [DependsOn(
        typeof(AppointmentDomainSharedModule)
    )]

    public class BaseDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
             
        }
    }
}
