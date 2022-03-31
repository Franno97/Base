using System;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class AuditableDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
         
        public string CategoriaId { get; set; }

        public string Categoria { get; set; }


        public AuditableTipo Tipo { get; set; }

      
        public string Item { get; set; }

    }

}
