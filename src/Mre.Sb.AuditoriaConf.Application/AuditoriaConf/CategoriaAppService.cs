using Mre.Sb.AuditoriaConf.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class CategoriaAppService :
    CrudAppService<
        Categoria,
        CategoriaDto,
        string>,
    ICategoriaAppService
    {
        private readonly CategoriaManager CategoriaManager;

        public CategoriaAppService(IRepository<Categoria, string> repository,
        CategoriaManager managerNameParameterManager)
            : base(repository)
        {
            this.CategoriaManager = managerNameParameterManager;

            //GetPolicyName = ModulePermissions.Categoria.Default;
            //GetListPolicyName = ModulePermissions.Categoria.Default;
            //CreatePolicyName = ModulePermissions.Categoria.Create;
            //UpdatePolicyName = ModulePermissions.Categoria.Update;
            //DeletePolicyName = ModulePermissions.Categoria.Delete;
        }

        public override async Task<CategoriaDto> CreateAsync(CategoriaDto input)
        {
            await CheckCreatePolicyAsync();

            await CategoriaManager.ValidateCreateAsync(input.Id);

            var entity = await MapToEntityAsync(input);

            TryToSetTenantId(entity);

            await Repository.InsertAsync(entity, autoSave: true);

            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task<ListResultDto<CategoriaDto>> GetLookupAsync()
        {
            await CheckGetListPolicyAsync();

            var list = await Repository.GetListAsync();

            return new ListResultDto<CategoriaDto>(
                ObjectMapper.Map<List<Categoria>, List<CategoriaDto>>(list)
            );
        }

    }
}
