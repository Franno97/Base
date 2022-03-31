using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using Mre.Sb.AuditoriaConf.Permisos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class AuditarAppService : ApplicationService, IAuditarAppService
    {
        private readonly AuditarManager auditarManager;
        private readonly IRepository<Auditar, Guid> repository;
        private readonly IRepository<Auditable, Guid> repositoryAuditable;

        public AuditarAppService(AuditarManager auditarManager,
            IRepository<Auditar, Guid> repository,
            IRepository<Auditable, Guid> repositoryAuditable)
        {
            this.auditarManager = auditarManager;
            this.repository = repository;
            this.repositoryAuditable = repositoryAuditable;

           
        }

        public async Task<AuditarObjetoDto> ObtenerAsync(string item)
        {
            var consultaAuditable = await repositoryAuditable.GetQueryableAsync();
            consultaAuditable = consultaAuditable.Where(a => a.Item == item);

            var consultaAuditar = await repository.GetQueryableAsync();


            var consulta = from auditable in consultaAuditable
                           join auditar in consultaAuditar
                           on auditable.Id equals auditar.AuditableId
                           select new AuditarObjetoDto()
                           {
                               Item = auditable.Item,
                               Acciones = auditar.Acciones,
                               Tipo = auditable.Tipo
                           }
                         ;

            var entidadDto = await AsyncExecuter.SingleOrDefaultAsync(consulta);

            return entidadDto;
        }

         
        public async  Task<bool> ConfigurarAsync(AuditarObjetoDto input)
        {
            await CheckPolicyAsync(AuditoriaConfPermissions.Auditar.Delete);

            return await auditarManager.ConfigurarAsync(tipo: input.Tipo, item: input.Item, acciones: input.Acciones); 

        }

      

        public async Task<ICollection<AuditarObjetoDto>> ObtenerListaAsync(string categoriaId)
        {

            var consultaAuditable = await repositoryAuditable.GetQueryableAsync();
            consultaAuditable = consultaAuditable.Where(a => a.CategoriaId == categoriaId);

            var consultaAuditar = await repository.GetQueryableAsync(); 


            var consulta = from auditable in consultaAuditable
                         join auditar in consultaAuditar
                         on  auditable.Id equals auditar.AuditableId
                         select new AuditarObjetoDto() { 
                             Item = auditable.Item,
                             Acciones = auditar.Acciones,
                             Tipo = auditable.Tipo
                         }
                         ;

            var lista = await AsyncExecuter.ToListAsync(consulta);

            return lista;
        }

        public async Task<bool> EliminarAsync(string item)
        {
            await CheckPolicyAsync(AuditoriaConfPermissions.Auditar.Delete);

            return await auditarManager.EliminarAsync(item: item);

        }

        public async Task<PagedResultDto<AuditarObjetoBuscarDto>> BuscarAsync(AuditarBuscarInputDto input)
        {
           
           

            var consultaAuditable = await repositoryAuditable.GetQueryableAsync();
            var consultaAuditar = await repository.GetQueryableAsync();


            var consulta = from auditable in consultaAuditable
                           join auditar in consultaAuditar
                           on auditable.Id equals auditar.AuditableId
                           select new
                           {
                               Item = auditable.Item,
                               CategoriaId = auditable.CategoriaId,
                               Categoria = auditable.Categoria.Nombre,
                               Acciones = auditar.Acciones,
                               Tipo = auditable.Tipo,
                               FechaCreacion = auditar.CreationTime
                           }
                         ;

            if (!input.CategoriaId.IsNullOrWhiteSpace()) {
                consulta = consulta.Where(a => a.CategoriaId == input.CategoriaId);
            }

            if (!input.Filtro.IsNullOrWhiteSpace())
            {
                consulta = consulta.Where(a => a.Item.ToUpper().StartsWith(input.Filtro.ToUpper()));
            }

            if (!input.Sorting.IsNullOrWhiteSpace())
            {
                consulta = consulta.OrderBy(input.Sorting);
            }
            else {
                consulta = consulta.OrderByDescending(e => e.FechaCreacion);
            }


            var total = await AsyncExecuter.CountAsync(consulta);

        
            consulta = consulta.PageBy(input);
            consulta = consulta.Take(input.MaxResultCount);

            var consultaDto = from data in consulta
                              select new AuditarObjetoBuscarDto()
                              {
                                  Categoria = data.Categoria,
                                  CategoriaId = data.CategoriaId,
                                  Item = data.Item,
                                  Acciones = data.Acciones,
                                  Tipo = data.Tipo
                              }
                         ; 

            var lista = await AsyncExecuter.ToListAsync(consultaDto);

            return new PagedResultDto<AuditarObjetoBuscarDto>(
                total,
                lista
            );
        } 

    }


}
