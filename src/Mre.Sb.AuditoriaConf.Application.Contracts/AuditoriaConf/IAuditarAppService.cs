using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public interface IAuditarAppService: IApplicationService
    {
        Task<AuditarObjetoDto> ObtenerAsync(string item);


        Task<ICollection<AuditarObjetoDto>> ObtenerListaAsync(string categoriaId);

        Task<PagedResultDto<AuditarObjetoBuscarDto>> BuscarAsync(AuditarBuscarInputDto input);


        Task<bool> ConfigurarAsync(AuditarObjetoDto input);

        Task<bool> EliminarAsync(string item);

    }
}
