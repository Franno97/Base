using System.Collections.Generic;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    
    public class CambioAuditarEventoEto
    {
        public string CategoriaId { get; set; }
        public string Item { get; set; }
        public ICollection<string> Acciones { get; set; }
        public TipoCambioAuditar TipoCambio { get; set; }
    }

    /// <summary>
    /// Evento local
    /// </summary>
    public class CambioAuditarEvento
    {
        public string CategoriaId { get; set; }
        public string Item { get; set; }
        public ICollection<string> Acciones { get; set; }
        public TipoCambioAuditar TipoCambio { get; set; }
    }
}
