using Mre.Sb.AuditoriaConf.Application;
using System;
using Volo.Abp.Modularity;

namespace Mre.Sb.AuditoriaConf
{
    [DependsOn(
         typeof(AuditoriaConfApplicationContractsModule)
         )]
    public class AuditoriaConfHttpApiModule : AbpModule
    {
       
    }
}
