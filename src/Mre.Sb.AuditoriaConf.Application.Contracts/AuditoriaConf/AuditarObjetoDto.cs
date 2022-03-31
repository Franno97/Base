using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class AuditarObjetoDto
    {
        [Required]
        public AuditableTipo Tipo { get; set; }

        [Required]
        [StringLength(AuditableConsts.MaxItemLongitud)]
        public string Item { get; set; }

        [Required]
        public ICollection<string> Acciones { get; set; }
    }


    public class AuditarObjetoBuscarDto
    {
        [Required]
        public AuditableTipo Tipo { get; set; }

        public string CategoriaId { get; set; }

        public string Categoria { get; set; }

        [Required]
        [StringLength(AuditableConsts.MaxItemLongitud)]
        public string Item { get; set; }

        [Required]
        public ICollection<string> Acciones { get; set; }
    }
}
