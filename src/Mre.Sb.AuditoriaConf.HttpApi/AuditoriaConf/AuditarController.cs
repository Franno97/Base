using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.AuditoriaConf.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    [RemoteService(Name = AuditoriaConfRemoteServiceConsts.RemoteServiceName)]
    [Area("AuditoriaConf")]
    [Route("api/auditoria-conf/auditar")]
    [ControllerName("Auditar")]
    [Authorize]
    public class AuditarController : AuditoriaConfBaseController, IAuditarAppService
    {
        public IAuditarAppService AuditarAppService { get; }


        public AuditarController(IAuditarAppService auditarAppService)
        {
            AuditarAppService = auditarAppService;
        }


        [HttpGet]
        public Task<ICollection<AuditarObjetoDto>> ObtenerListaAsync(string categoriaId)
        {
            return AuditarAppService.ObtenerListaAsync(categoriaId);
        }

        [HttpGet]
        [Route("{item}")]
        public Task<AuditarObjetoDto> ObtenerAsync(string item)
        {
            return AuditarAppService.ObtenerAsync(item);
        }

        [HttpGet]
        [Route("buscar")]
        public Task<PagedResultDto<AuditarObjetoBuscarDto>> BuscarAsync(AuditarBuscarInputDto input)
        {
            return AuditarAppService.BuscarAsync(input);
        }

        [HttpPost]
        public Task<bool> ConfigurarAsync(AuditarObjetoDto input)
        {
            return AuditarAppService.ConfigurarAsync(input);
        }


        [HttpDelete]
        [Route("{item}")]
        public Task<bool> EliminarAsync(string item)
        {
            return AuditarAppService.EliminarAsync(item);
        }

      
    }
     
}
