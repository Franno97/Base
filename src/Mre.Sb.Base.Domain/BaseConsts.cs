namespace Mre.Sb.Base
{
    public static class BaseConsts
    {
        public const string DbTablePrefix = "App";

        public const string DbSchema = null;

        public static class Notificaciones {

            public const string CreacionUsuarioInterno = "Usuario.Creacion.Interno";

            public const string CreacionUsuarioExterno = "Usuario.Creacion.Externo";

            public const string UsuarioClaveRecuperacion= "Usuario.Clave.Recuperacion";

            public const string UsuarioAccesosFallidos = "Usuario.Accesos.Fallidos";

        }

        public static class Claims {

            public const string UnidadAdministrativa = "Mre.UnidadAdministrativa";

            public const string Permiso = "Mre.Pms";
        
        }
    }
}
