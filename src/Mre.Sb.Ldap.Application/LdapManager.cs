using LdapForNet;
using LdapForNet.Native;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace Mre.Sb.Ldap
{

    public class LdapManager : DomainService
    {
        private readonly ILogger<LdapManager> logger;

        public LdapOptions LdapOpciones { get; }


        public LdapManager(IOptions<LdapOptions> ldapOpciones,
            ILogger<LdapManager> logger)
        {
            LdapOpciones = ldapOpciones.Value;
            this.logger = logger;
        }

        
        public async Task<UsuarioLdap> BuscarUsuarioAsync(string usuarioOEmail)
        {
            logger.LogInformation("Buscar usuario {usuarioOEmail} en Ldap", usuarioOEmail);

            var usuario = usuarioOEmail;
            if (LdapOpciones.NormalizarNombreUsuario && EmailHelper.IsValidEmail(usuario))
            { 
                usuario = EmailHelper.GetUserNameFromEmail(usuarioOEmail);
            }

            UsuarioLdap usuarioEncontrado = null; 
            using (var ldapConnection = new LdapConnection())
            {
                ldapConnection.Connect(LdapOpciones.Servidor, LdapOpciones.Puerto,
                    Native.LdapSchema.LDAP, Native.LdapVersion.LDAP_VERSION3);

                await ldapConnection.BindAsync(Native.LdapAuthType.Simple, new LdapCredential
                {
                    UserName = $"cn={LdapOpciones.UsuarioBind},{LdapOpciones.BaseDc}", 
                    Password = LdapOpciones.ClaveBind
                });


                var filtroBuscarUsuarios = string.Format(LdapOpciones.FiltroBuscarUsuarios, usuario);
                
                logger.LogDebug("Buscar usuario con los valores: BaseDc {baseDc},  Filtro {filtroBuscarUsuarios}", LdapOpciones.BaseDc,  filtroBuscarUsuarios);

                var searchResults = await ldapConnection.SearchAsync(LdapOpciones.BaseDc, filtroBuscarUsuarios);
                var usuarioEntidad = searchResults.FirstOrDefault();

                if (usuarioEntidad != null)
                {
                    logger.LogInformation("Buscar usuario {usuarioOEmail} en Ldap. Encontrado", usuarioOEmail);

                    usuarioEncontrado = MapearEntidadLdapUsuario(usuarioEntidad);
                }
                else {
                    logger.LogInformation("Buscar usuario {usuarioOEmail} en Ldap. No Encontrado", usuarioOEmail);
                }
            }

            return usuarioEncontrado;
        }

        private UsuarioLdap MapearEntidadLdapUsuario(LdapEntry usuarioEntidad)
        {
            var usuario = new UsuarioLdap();

          
            var atributos = usuarioEntidad.ToDirectoryEntry().Attributes;
            var usuarioAtributo = atributos.SingleOrDefault(a => a.Name.ToUpper() == LdapOpciones.MapeoUsuario.ToUpper());
            if (usuarioAtributo != null)
            {
                usuario.Usuario = usuarioAtributo.GetValue<string>();
            }

            var email = atributos.SingleOrDefault(a => a.Name.ToUpper() == LdapOpciones.MapeoEmail.ToUpper());
            if (email != null) {
                usuario.Email = email.GetValue<string>();
            }

            var apellido = atributos.SingleOrDefault(a => a.Name.ToUpper() == LdapOpciones.MapeoApellido.ToUpper());
            if (apellido != null)
            {
                usuario.Apellido = apellido.GetValue<string>();
            }

            var nombre = atributos.SingleOrDefault(a => a.Name.ToUpper() == LdapOpciones.MapeoNombre.ToUpper());
            if (nombre != null)
            {
                usuario.Nombre = nombre.GetValue<string>();
            }
            else {
                var nombresCompletos = atributos.SingleOrDefault(a => a.Name.ToUpper() == LdapOpciones.MapeoNombresCompletos.ToUpper());

                if (nombresCompletos != null && !string.IsNullOrWhiteSpace(usuario.Apellido))
                {
                    var nombresProcesar = nombresCompletos.GetValue<string>();

                    int index = nombresProcesar.LastIndexOf(usuario.Apellido);
                    usuario.Nombre = (index < 0)
                        ? string.Empty
                        : nombresProcesar.Remove(index, usuario.Apellido.Length).Trim();
                }
            }

            return usuario;
        }
    }
}
