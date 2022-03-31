using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Mre.Sb.Ldap
{
    [DependsOn(
        typeof(LdapApplicationContractsModule)
        )]
    public class LdapApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
            var configuration = context.Services.GetConfiguration();
            Configure<LdapOptions>(configuration.GetSection("Ldap")); 
        }
    }
}
