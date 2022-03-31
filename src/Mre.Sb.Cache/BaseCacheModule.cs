using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Mre.Sb.Cache
{
    public class BaseCacheModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = configuration["Cache:KeyPrefix"];
            });

            Configure<PersonalizarDistributedCacheOptions>(
                configuration.GetSection("Cache"));
        }
    }
}
