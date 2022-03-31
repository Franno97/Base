using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.Base.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Mre.Sb.Base.Identidad
{
     

    [RemoteService(Name = "Identidad")]
    [Area("identidad")]
    [ControllerName("Usuario")]
    [Route("api/identidad/usuario")]
    [Authorize]
    public class UsuarioController : BaseController, IUsuarioAppService
    {
        protected IUsuarioAppService UsuarioAppService { get; }

        protected IIdentityUserAppService UserAppService { get; }

        public UsuarioController(IUsuarioAppService usuarioAppService)
        { 
            UsuarioAppService = usuarioAppService;
        }

         
        [HttpPost]
        public virtual Task<IdentityUserDto> CrearAsync(UsuarioCrearDto input)
        {
            return UsuarioAppService.CrearAsync(input);
        }

        [HttpGet]
        public virtual Task<IReadOnlyList<IdentityUserDto>> ObtenerListaAsync(List<Guid> input)
        {
            return UsuarioAppService.ObtenerListaAsync(input);
        }
    }
}
