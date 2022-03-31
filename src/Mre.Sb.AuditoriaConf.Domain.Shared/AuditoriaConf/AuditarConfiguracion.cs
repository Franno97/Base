namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public static class AuditarConfiguracion {

        /// <summary>
        /// Configuracion, para establecer, si se realizan pubicacion eventos 
        /// por cambios en la entidad auditar. 
        /// Eventos Distribuidos
        /// </summary>
        public static bool PublicarEventoDistribuidoCambioAuditar { get; set; } = false;

        /// <summary>
        /// Clave cache, que es utilizada por la libreria "Mre.Sb.Auditar" para aplicar auditoria, 
        /// en los diferentes componentes. 
        /// </summary>
        public static string ClienteCacheClave { get; set; } = "Mre.Sb.Auditar.Configuracion";
    }
}
