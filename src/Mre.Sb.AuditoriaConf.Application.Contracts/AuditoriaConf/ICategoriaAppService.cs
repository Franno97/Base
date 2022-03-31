using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public interface ICategoriaAppService : ICrudAppService<CategoriaDto, string>
    {
        Task<ListResultDto<CategoriaDto>> GetLookupAsync();
    }
}
