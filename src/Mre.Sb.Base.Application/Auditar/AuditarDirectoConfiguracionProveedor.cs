using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mre.Sb.Auditar;
using Mre.Sb.AuditoriaConf.AuditoriaConf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using AuditarObjetoDto = Mre.Sb.AuditoriaConf.AuditoriaConf.AuditarObjetoDto;

namespace Mre.Sb.Base.Auditar
{
    /// <summary>
    /// Implementacion personalizada mecanismo obtener la configuracion elementos auditar.
    /// La libreria utiliza REST API. Esta implementacion consume directamente servicio aplicacion correspondiente.
    /// </summary>
    public class AuditarDirectoConfiguracionProveedor : IAuditarConfiguracionProveedor
    {
       
       
        private readonly AuditoriaConfiguracion auditoriaConfiguracion;
        private readonly IAuditarAppService auditarAppService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDistributedCache distributedCache;
        private readonly IDistributedCacheSerializer serializadorCache;
        private readonly ILogger<AuditarDirectoConfiguracionProveedor> logger;

        public AuditarDirectoConfiguracionProveedor(
            IAuditarAppService auditarAppService,
            IOptions<AuditoriaConfiguracion> optionAuditoria,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache distributedCache,
            IDistributedCacheSerializer serializadorCache,
            ILogger<AuditarDirectoConfiguracionProveedor> logger)
        {
            this.auditoriaConfiguracion = optionAuditoria.Value;
            this.auditarAppService = auditarAppService;
            this.httpContextAccessor = httpContextAccessor;
            this.distributedCache = distributedCache;
            this.serializadorCache = serializadorCache;
            this.logger = logger;
        }


        public async Task<bool> AuditarEntidadAsync(Type tipo)
        {

            logger.LogDebug("Verificar entidad si se audita. tipo {tipo}", tipo.FullName);

            var entidadesAuditar = await ObtenerListaAuditarObjeto();
            var existe = entidadesAuditar.Any(a => a.Item.ToUpper() == tipo.FullName.ToUpper());

            logger.LogDebug("Verificar entidad si se audita. tipo {tipo}. Resultado {resultado}", tipo.FullName, existe);


            return existe;

        }

        public async Task<bool> AuditarEntidadAccionAsync(Type tipo, string accion)
        {
            logger.LogDebug("Verificar entidad, accion si se audita. Tipo {tipo}. Accion {accion}", tipo.FullName, accion);

            var entidadesAuditar = await ObtenerListaAuditarObjeto();
            var configuracion = entidadesAuditar.SingleOrDefault(a => a.Item.ToUpper() == tipo.FullName.ToUpper());
            if (configuracion == null)
                return false;

            var resultado = configuracion.Acciones.Contains(MapearAccion(accion));

            logger.LogDebug("Verificar entidad, accion si se audita. Tipo {tipo}. Accion {accion}. Resultado {resultado}", tipo.FullName, accion, resultado);

            return resultado;
        }


        protected string MapearAccion(string accionInterna)
        {

            switch (accionInterna.ToUpper())
            {
                case "INSERT":
                    return "C";
                case "UPDATE":
                    return "A";
                case "DELETE":
                    return "E";
            }

            return string.Empty;
        }

        protected virtual async Task<ICollection<AuditarObjetoDto>> ObtenerListaAuditarObjeto()
        {

            var cacheKey = CrearClaveCache(auditoriaConfiguracion.ConfiguracionCategoriaCodigo);
            var httpContext = httpContextAccessor?.HttpContext;

            if (httpContext != null && httpContext.Items[cacheKey] is ICollection<AuditarObjetoDto> listaAuditarObjeto)
            {
                return listaAuditarObjeto;
            }

            var listaCacheada = await distributedCache.GetAsync(cacheKey);

            if (listaCacheada != null)
            {

                logger.LogDebug("Recuperar configuracion auditoria (Cache). Categoria {Categoria}. Tipo Cache {cacheTipo}", auditoriaConfiguracion.ConfiguracionCategoriaCodigo, distributedCache.GetType());

                var listaDeserializada = serializadorCache.Deserialize<ICollection<AuditarObjetoDto>>(listaCacheada);

                if (httpContext != null)
                {
                    httpContext.Items[cacheKey] = listaDeserializada;
                }

                return listaDeserializada;
            }


            logger.LogDebug("Recuperar configuracion auditoria (API). Categoria {Categoria}", auditoriaConfiguracion.ConfiguracionCategoriaCodigo);

            var optionesCache = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(auditoriaConfiguracion.CacheTiempo));


            ICollection<AuditarObjetoDto> resultado;
            try
            {

                resultado = await  auditarAppService.ObtenerListaAsync(auditoriaConfiguracion.ConfiguracionCategoriaCodigo);

  
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al recuperar auditoria. Categoria {CategoriaAuditoria}", auditoriaConfiguracion.ConfiguracionCategoriaCodigo);
                return new List<AuditarObjetoDto>();
            }


            await distributedCache.SetAsync(cacheKey, serializadorCache.Serialize(resultado), optionesCache);

            if (httpContext != null)
            {
                httpContext.Items[cacheKey] = resultado;
            }

            return resultado;

        }

        protected virtual string CrearClaveCache(string categoriaCodigo)
        {
            return $"{auditoriaConfiguracion.CacheClave}_{categoriaCodigo}";
        }


    }
}
