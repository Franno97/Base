using Mre.Sb.AuditoriaConf.Application;
using Mre.Sb.Cita;
using Mre.Sb.RegistroPersona;
using Mre.Sb.UnidadAdministrativa;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.Base
{
    [DependsOn(
        typeof(BaseDomainSharedModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule)
    )]
    //Permissions
    [DependsOn(
        typeof(AuditoriaConfApplicationContractsModule)
    )]
    [DependsOn(
        typeof(AdministrativeUnitApplicationContractsModule)
    )]
    [DependsOn(
        typeof(PersonRegistrationApplicationContractsModule)
    )]
    [DependsOn(
        typeof(AppointmentApplicationContractsModule)
    )]
    public class BaseApplicationContractsModule : AbpModule
    {
         
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BaseDtoExtensions.Configure();
        }
    }
}
