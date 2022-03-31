using Microsoft.Extensions.Localization;
using Mre.Sb.AuditoriaConf.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class AuditarManager : DomainService
    {
        private readonly IStringLocalizer<AuditoriaConfResource> localizer;

        private readonly IRepository<Auditar, Guid> repository;

        private readonly IRepository<Auditable, Guid> repositoryAuditable;
        private readonly IDistributedEventBus distributedEventBus;
        private readonly ILocalEventBus localEventBus;

        public AuditarManager(IRepository<Auditar, Guid> repository,
            IRepository<Auditable, Guid> repositoryAuditable,
            IDistributedEventBus distributedEventBus,
            ILocalEventBus localEventBus,
            IStringLocalizer<AuditoriaConfResource> localizer)
        {
            this.repository = repository;
            this.repositoryAuditable = repositoryAuditable;
            this.distributedEventBus = distributedEventBus;
            this.localEventBus = localEventBus;
            this.localizer = localizer;

        }


        public async Task<bool> ConfigurarAsync(AuditableTipo tipo, string item, ICollection<string> acciones)
        {

            //1. Validaciones
            //- Si existe el item
            //- Si las acciones, son permitidas para el tipo.
            //- Si existe actualizar, sino crear.
            //La accion de eliminar, es explicita, o si no se pasa acciones  seelimina.

            //Obtener objeto audiable
            var consultaAuditable = await repositoryAuditable.GetQueryableAsync();
            consultaAuditable =  consultaAuditable.Where(a => a.Item.ToUpper() == item.ToUpper());

            var auditable = await AsyncExecuter.SingleOrDefaultAsync(consultaAuditable);
             

            if (auditable == null)
            {
                throw new UserFriendlyException(string.Format(localizer["Auditar:NoExiste"], item));
            }


            var consultaAuditar = await repository.GetQueryableAsync();
            consultaAuditar = consultaAuditar.Where(a => a.AuditableId == auditable.Id);
             
            var auditar = await AsyncExecuter.SingleOrDefaultAsync(consultaAuditar);
            if (auditar == null)
            {

                auditar = new Auditar(id: GuidGenerator.Create(), auditableId: auditable.Id, acciones: acciones);
                await repository.InsertAsync(auditar, autoSave: true);

                await PublicarEvento(auditable, auditar, TipoCambioAuditar.Creacion);
            }
            else {

                auditar.Acciones = acciones; 
                await repository.UpdateAsync(auditar, autoSave: true);

                await PublicarEvento(auditable, auditar, TipoCambioAuditar.Actualizacion);
            }

            return true;

        }

        

        public async Task<bool> EliminarAsync(string item) {

            var consultaAuditable = await repositoryAuditable.GetQueryableAsync();
            consultaAuditable = consultaAuditable.Where(a => a.Item.ToUpper() == item.ToUpper());

            var auditable = await AsyncExecuter.SingleOrDefaultAsync(consultaAuditable);


            if (auditable == null)
            {
                throw new UserFriendlyException(string.Format(localizer["Auditar:NoExiste"], item));
            }


            var consultaAuditar = await repository.GetQueryableAsync();
            consultaAuditar = consultaAuditar.Where(a => a.AuditableId == auditable.Id);

            var auditar = await AsyncExecuter.SingleOrDefaultAsync(consultaAuditar);

            if (auditar != null)
            { 
                await repository.DeleteAsync(auditar, autoSave: true);

                await PublicarEvento(auditable, auditar, TipoCambioAuditar.Eliminacion);
            }

            return true;
        }


        private async Task PublicarEvento(Auditable auditable, Auditar auditar, TipoCambioAuditar tipoCambio)
        {
            if (AuditarConfiguracion.PublicarEventoDistribuidoCambioAuditar)
            {

                await distributedEventBus.PublishAsync(
                                new CambioAuditarEventoEto
                                {
                                    TipoCambio = tipoCambio,
                                    CategoriaId = auditable.CategoriaId,
                                    Item = auditable.Item,
                                    Acciones = auditar.Acciones,
                                }
                            );

            }
            else {
                await localEventBus.PublishAsync(
                                   new CambioAuditarEvento
                                   {
                                       TipoCambio = tipoCambio,
                                       CategoriaId = auditable.CategoriaId,
                                       Item = auditable.Item,
                                       Acciones = auditar.Acciones,
                                   }
                               );
            }
             
        }
  
    }

   

}
