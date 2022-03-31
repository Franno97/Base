namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public static class CategoriaConsts {

         
        public const int MaxIdLongitud = 6;
    }


    public static class AuditoriaConfDbProperties
    {
        public static string DbTablePrefix { get; set; } = "";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Auditoria";
    }
}
