using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.Base.Controllers;
using Mre.Sb.Base.Identidad;
using System.Threading.Tasks;
using Volo.Abp;

namespace Mre.Sb.Base.Identidad
{
    [RemoteService(Name = "Identidad")]
    [Area("identidad")]
    [ControllerName("IdentidadConfiguracion")]
    [Route("api/identidad/identidad-configuracion")]
    [Authorize]
    public class IdentidadConfiguracionController : BaseController, IIdentidadConfiguracionAppService
    {
        protected IIdentidadConfiguracionAppService IdentidadConfiuracionAppService { get; }


        public IdentidadConfiguracionController(IIdentidadConfiguracionAppService identidadConfiuracionAppService)
        {
            IdentidadConfiuracionAppService = identidadConfiuracionAppService;
        }

        [HttpPost]
        public  Task ActualizarAsync(ActualizarIdentidadConfiguracionDtoDto input)
        {
            return IdentidadConfiuracionAppService.ActualizarAsync(input);
        }

        [HttpGet]
        public  Task<IdentidadConfiguracionDto> ObtenerAsync()
        {
            return IdentidadConfiuracionAppService.ObtenerAsync();
        }
    }
}
