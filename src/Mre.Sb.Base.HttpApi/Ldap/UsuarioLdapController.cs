using Microsoft.AspNetCore.Mvc;
using Mre.Sb.Base.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Mre.Sb.Ldap
{
    [RemoteService(Name = "Ldap")]
    [Area("ldap")]
    [ControllerName("UsuarioLdap")]
    [Route("api/ldap/usuario-ldap")]
    public class UsuarioLdapController : BaseController, ILdapAppService
    {
        public ILdapAppService LdapAppService { get; }

        public UsuarioLdapController(ILdapAppService  ldapAppService)
        {
            LdapAppService = ldapAppService;
        }

     
        [HttpGet]
        public Task<UsuarioLdap> BuscarUsuarioAsync(string usuario)
        {
            return LdapAppService.BuscarUsuarioAsync(usuario);
        }
         
    }
}
