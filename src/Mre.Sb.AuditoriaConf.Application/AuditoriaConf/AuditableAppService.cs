using Mre.Sb.AuditoriaConf.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class AuditableAppService :
    CrudAppService<
        Auditable,
        AuditableDto,
        Guid,
        ObtenerAuditableInput,
        CrearActualizarAuditableDto>,
    IAuditableAppService
    {
     
        public AuditableAppService(IRepository<Auditable, Guid> repository)
            : base(repository)
        {
        
            //GetPolicyName = ModulePermissions.Auditable.Default;
            //GetListPolicyName = ModulePermissions.Auditable.Default;
            //CreatePolicyName = ModulePermissions.Auditable.Create;
            //UpdatePolicyName = ModulePermissions.Auditable.Update;
            //DeletePolicyName = ModulePermissions.Auditable.Delete;
        }


        public override async Task<AuditableDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(a => a.Id == id);

            var queryableDto = queryable.Select(entity => new AuditableDto()
            {
                Id = entity.Id,
                Categoria = entity.Categoria.Nombre,
                CategoriaId = entity.CategoriaId,
                Tipo = entity.Tipo,
                Item = entity.Item
            });
             

            var entityDto = await AsyncExecuter.SingleOrDefaultAsync(queryableDto);
 

            return entityDto;
        }

        public override async Task<PagedResultDto<AuditableDto>> GetListAsync(ObtenerAuditableInput input)
        {

            await CheckGetListPolicyAsync();

            var consulta = await CreateFilteredQueryAsync(input);

            consulta = consulta
                .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Item.ToUpper().Contains(input.Filter.ToUpper())
                    )
              .WhereIf(
                        !input.CategoriaId.IsNullOrWhiteSpace(),
                        u =>
                            u.CategoriaId == input.CategoriaId
                    );

            var totalCount = await AsyncExecuter.CountAsync(consulta);

            consulta = ApplySorting(consulta, input);
            consulta = ApplyPaging(consulta, input);


            var consultaDto = consulta.Select(entity => new AuditableDto()
            {
                Id = entity.Id,
                Categoria = entity.Categoria.Nombre,
                CategoriaId = entity.CategoriaId,
                Tipo = entity.Tipo,
                Item = entity.Item
            });

            var entityDtos = await AsyncExecuter.ToListAsync(consultaDto); 
          
            return new PagedResultDto<AuditableDto>(
                totalCount,
                entityDtos
            );
        }


        public override async Task<AuditableDto> CreateAsync(CrearActualizarAuditableDto input)
        {
            await CheckCreatePolicyAsync();

           
            var entity = await MapToEntityAsync(input);

            TryToSetTenantId(entity);

            await Repository.InsertAsync(entity, autoSave: true);


            return await GetAsync(entity.Id);
        }

        
        public override async Task<AuditableDto> UpdateAsync(Guid id, CrearActualizarAuditableDto input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);
           
            await MapToEntityAsync(input, entity);
            await Repository.UpdateAsync(entity, autoSave: true);

            return await GetAsync(entity.Id);
        }
    }

}
