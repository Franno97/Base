using System.Collections.Generic;
using Volo.Abp.Caching;

namespace Mre.Sb.Cache
{
    public class PersonalizarDistributedCacheOptions : AbpDistributedCacheOptions {

        public ICollection<string> ExcluirAplicarPrefijo { get; set; } = new List<string>();

    }
}
