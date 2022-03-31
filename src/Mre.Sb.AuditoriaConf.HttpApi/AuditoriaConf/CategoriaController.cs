using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.AuditoriaConf.Application;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{


    [RemoteService(Name = AuditoriaConfRemoteServiceConsts.RemoteServiceName)]
    [Area("AuditoriaConf")]
    [Route("api/auditoria-conf/categoria")]
    [ControllerName("Categoria")]
    [Authorize]
    public class CategoriaController : AuditoriaConfBaseController, ICategoriaAppService
    {
        public ICategoriaAppService CategoriaAppService { get; }


        public CategoriaController(ICategoriaAppService categoriaAppService)
        {
            CategoriaAppService = categoriaAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<CategoriaDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return CategoriaAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("lookup")]
        public virtual Task<ListResultDto<CategoriaDto>> GetLookupAsync()
        {
            return CategoriaAppService.GetLookupAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<CategoriaDto> GetAsync(string id)
        {
            return CategoriaAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<CategoriaDto> CreateAsync(CategoriaDto input)
        {
            return CategoriaAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<CategoriaDto> UpdateAsync(string id, CategoriaDto input)
        {
            return CategoriaAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return CategoriaAppService.DeleteAsync(id);
        }
    }
     
}
