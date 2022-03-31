namespace Mre.Sb.Base.Settings
{
    public static class BaseConfiguraciones
    {
        private const string ModuloNombre = "Base";

        public static class Institucion
        {
            public const string GrupoNombre = ModuloNombre + ".Institucion";
            public const string Direccion = GrupoNombre + ".Direccion"; 
        }

        public static class Accesos
        {
            public const string GrupoNombre = ModuloNombre + ".Accesos";
            public const string NotificarAccesoFallido = GrupoNombre + ".NotificarFallido";
        }

        public static class Identidad
        {
            public const string GrupoNombre = ModuloNombre + ".Identidad";
            public const string ControlarClavesAnterior = GrupoNombre + ".ControlarClavesAnterior";
            public const string ControlarClavesAnteriorCantidad = GrupoNombre + ".ControlarClavesAnteriorCantidad";
             
        }
    }

}