using System.Threading.Tasks;

namespace Mre.Sb.Ldap
{
    public interface ILdapAppService
    {
        Task<UsuarioLdap> BuscarUsuarioAsync(string usuario);
    }
}
