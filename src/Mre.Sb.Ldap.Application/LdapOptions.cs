namespace Mre.Sb.Ldap
{
    public class LdapOptions
    {
        
        public string Servidor { get; set; }

        public int Puerto { get; set; } = 389;

        public string BaseDc { get; set; }

        /// <summary>
        /// BindDN
        /// </summary>
        public string UsuarioBind { get; set; }

        /// <summary>
        ///BindPassword
        /// </summary>
        public string ClaveBind { get; set; }


        /// <summary>
        /// Normalizar el nombre usuario, si parametro es un correo electronico,
        /// quitar el dominio, para obtener unicamente el usuario para realizar la busqueda
        /// establecer en true para aplicar la normalizacion
        /// </summary>
        public bool NormalizarNombreUsuario { get; set; } = false;

        public string MapeoUsuario { get; set; } = "uid";

        public string MapeoEmail { get; set; } = "mail";

        public string MapeoNombre { get; set; } = "givenName";

        public string MapeoApellido { get; set; } = "sn";

        public string MapeoNombresCompletos { get; set; } = "cn";

        public string FiltroBuscarUsuarios { get; set; } = "(&(uid={0}))";
    }
}
