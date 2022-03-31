using Volo.Abp.Application.Dtos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class AuditarBuscarInputDto : PagedAndSortedResultRequestDto {

        public string CategoriaId { get; set; }

        public string Filtro { get; set; }

    }
}
