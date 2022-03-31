using System.Threading.Tasks;
using Volo.Abp.EventBus;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class LimpiarCacheLibreriaAuditarHandler
       : ILocalEventHandler<CambioAuditarEvento>,
         ITransientDependency
    {
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<LimpiarCacheLibreriaAuditarHandler> logger;

        public LimpiarCacheLibreriaAuditarHandler(IDistributedCache distributedCache,
            ILogger<LimpiarCacheLibreriaAuditarHandler> logger)
        {
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        public async Task HandleEventAsync(CambioAuditarEvento eventData)
        {
            var cacheKey = CrearClaveCache(eventData.CategoriaId);

            await distributedCache.RemoveAsync(cacheKey);

            logger.LogDebug("Limpiar cache Libreria Auditar. Categoria {categoria}. Clave cache {claveCache}"
                , eventData.CategoriaId, cacheKey);

        }

        protected virtual string CrearClaveCache(string categoriaCodigo)
        {
            return $"{AuditarConfiguracion.ClienteCacheClave}_{categoriaCodigo}";
        }

    }


}
