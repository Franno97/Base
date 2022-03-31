using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public interface IAuditableAppService : ICrudAppService<AuditableDto,
        Guid,
        ObtenerAuditableInput, CrearActualizarAuditableDto>
    {

    }

}
