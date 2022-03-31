using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Mre.Sb.Cache
{
    [Dependency(ReplaceServices = true)]
    //[ExposeServices(typeof(IDistributedCacheKeyNormalizer))]
    public class PersonalizarDistributedCacheKeyNormalizer : IDistributedCacheKeyNormalizer, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        protected PersonalizarDistributedCacheOptions DistributedCacheOptions { get; }

        public PersonalizarDistributedCacheKeyNormalizer(
            ICurrentTenant currentTenant,
            IOptions<PersonalizarDistributedCacheOptions> distributedCacheOptions)
        {
            CurrentTenant = currentTenant;
            DistributedCacheOptions = distributedCacheOptions.Value;
        }

        public virtual string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
        {
            var normalizedKey = $"c:{args.CacheName},k:{DistributedCacheOptions.KeyPrefix}{args.Key}";

            if (DistributedCacheOptions.ExcluirAplicarPrefijo.Contains(args.CacheName)) {
                normalizedKey = $"c:{args.CacheName},k:{args.Key}";
            }


            if (!args.IgnoreMultiTenancy && CurrentTenant.Id.HasValue)
            {
                normalizedKey = $"t:{CurrentTenant.Id.Value},{normalizedKey}";
            }

            return normalizedKey;
        }
    }
}
