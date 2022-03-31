using Volo.Abp.Application.Dtos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class ObtenerAuditableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CategoriaId { get; set; }
    }
}
