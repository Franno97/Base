using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    
    public class Auditable : Entity<Guid>
    {
        [Required]
        public string CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        [Required]
        public AuditableTipo Tipo { get; set; }

        [Required]
        [StringLength(AuditableConsts.MaxItemLongitud)]
        public string Item { get; set; }
    }
}
