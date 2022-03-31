using Mre.Sb.AuditoriaConf.Localization;
using System;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Mre.Sb.AuditoriaConf
{
    
    public class AuditoriaConfDomainSharedModule  : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AuditoriaConfDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AuditoriaConfResource>("es")
                    .AddVirtualJson("/Localization/AuditoriaConf");
                 
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("AuditoriaConf", typeof(AuditoriaConfResource));
            });
        }
    }
}
