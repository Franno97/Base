using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Mre.Sb.Ldap
{
    public class LdapAppService : ITransientDependency , ILdapAppService
    {
        private readonly ILogger<LdapAppService> logger;

        public LdapManager LdapManager { get; }
       
        public LdapAppService(LdapManager ldapManager,
            ILogger<LdapAppService> logger)
        {
            LdapManager = ldapManager;
            this.logger = logger;
        }

       

        public Task<UsuarioLdap> BuscarUsuarioAsync(string usuario)
        {
            return LdapManager.BuscarUsuarioAsync(usuario);
        }
    }
}
