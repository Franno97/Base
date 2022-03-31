using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.AuditoriaConf.Application;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    [RemoteService(Name = AuditoriaConfRemoteServiceConsts.RemoteServiceName)]
    [Area("AuditoriaConf")]
    [Route("api/auditoria-conf/auditable")]
    [ControllerName("Auditable")]
    [Authorize] 
    public class AuditableController : AuditoriaConfBaseController, IAuditableAppService
    {
        public IAuditableAppService AuditableAppService { get; }


        public AuditableController(IAuditableAppService auditableAppService)
        {
            AuditableAppService = auditableAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<AuditableDto>> GetListAsync(ObtenerAuditableInput input)
        {
            return AuditableAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<AuditableDto> GetAsync(Guid id)
        {
            return AuditableAppService.GetAsync(id);
        }



        [HttpPost]
        public virtual Task<AuditableDto> CreateAsync(CrearActualizarAuditableDto input)
        {
            return AuditableAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<AuditableDto> UpdateAsync(Guid id, CrearActualizarAuditableDto input)
        {
            return AuditableAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return AuditableAppService.DeleteAsync(id);
        }
    }
     
}
