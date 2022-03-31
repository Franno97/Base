using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Sb.Base.Cuenta
{
    public class OpenIdConfiguracion
    {
        public string ProveedorNombre { get; set; }

        public string NombreVisualizar { get; set; }

        public string Autoridad { get; set; }

        public string ClienteId { get; set; }

        public string ClienteClave { get; set; }

        public string UrlRetorno { get; set; }

        public string ClaimMapeoUsuario { get; set; } = "sub";
    }
}
