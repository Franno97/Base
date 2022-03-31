using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mre.Sb.Auditar;
using Mre.Sb.Base.Auditar;
using Mre.Sb.Base.Identidad;
using Mre.Sb.Base.Permiso;
using Mre.Sb.Ldap;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Mre.Sb.Base
{
    [DependsOn(
        typeof(BaseDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(BaseApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(LdapApplicationContractsModule)
        )]

    [DependsOn(
        typeof(AbpIdentityModelModule))]
    public class BaseApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BaseApplicationModule>();
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.AutoEventSelectors.Add<PermissionGrant>();
                options.EtoMappings.Add<PermissionGrant, PermissionGrantEto>();
                 
            });


            //Agregar validaciones personalizadas
            context.Services.AddScoped<IPasswordValidator<Volo.Abp.Identity.IdentityUser>, HistoricoPasswordValidator<Volo.Abp.Identity.IdentityUser>>();


        }

       

    }
}
