using Microsoft.AspNetCore.Mvc;
using Mre.Sb.Base.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Mre.Sb.Base.Identidad
{
    [RemoteService(Name = "Identidad")]
    [Area("identidad")]
    [ControllerName("PoliticaAutorizacion")]
    [Route("api/identidad/politica-autorizacion")]
    public class PoliticaAutorizacionController : BaseController, IPoliticaAutorizacionAppService
    {
        private readonly IPoliticaAutorizacionAppService politicaAutorizacionAppService;

        public PoliticaAutorizacionController(IPoliticaAutorizacionAppService politicaAutorizacionAppService)
        {
            this.politicaAutorizacionAppService = politicaAutorizacionAppService;
        }


        [HttpGet]
        public Task<Dictionary<string, bool>> ObtenerListaAsync()
        {
            return politicaAutorizacionAppService.ObtenerListaAsync();
        }
    }
}
