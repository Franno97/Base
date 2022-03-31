using System.ComponentModel.DataAnnotations;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class CrearActualizarAuditableDto 
    {
        
        [Required]
        public string CategoriaId { get; set; }
         
        [Required]
        public AuditableTipo Tipo { get; set; }

        [Required]
        [StringLength(AuditableConsts.MaxItemLongitud)]
        public string Item { get; set; }

    }

}
