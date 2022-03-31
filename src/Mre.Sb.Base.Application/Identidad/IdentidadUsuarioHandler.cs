using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Mre.Sb.Base.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Linq;
using Volo.Abp.SettingManagement;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Mre.Sb.Base.Identidad
{
    public class IdentidadUsuarioHandler
       : ILocalEventHandler<EntityChangedEventData<IdentityUser>>,
         ITransientDependency
    {
        private readonly IRepository<UsuarioHistorico, Guid> repository;
        private readonly ISettingManager settingManager;
        private readonly IdentityUserManager userManager;
        private readonly IGuidGenerator guidGenerator;
        private readonly IAsyncQueryableExecuter asyncExecuter;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly ILogger<IdentidadUsuarioHandler> logger;

        public IdentidadUsuarioHandler(IRepository<UsuarioHistorico, Guid> repository,
            ISettingManager settingManager,
            IdentityUserManager identityUserManager,
            IGuidGenerator guidGenerator,
            IAsyncQueryableExecuter asyncExecuter,
            IUnitOfWorkManager unitOfWorkManager,
            ILogger<IdentidadUsuarioHandler> logger)
        {
            this.repository = repository;
            this.settingManager = settingManager;
            this.userManager = identityUserManager;
            this.guidGenerator = guidGenerator;
            this.asyncExecuter = asyncExecuter;
            this.unitOfWorkManager = unitOfWorkManager;
            this.logger = logger;
        }

        
        public async Task HandleEventAsync(EntityChangedEventData<IdentityUser> eventData)
        {
            var controlarClavesAnterior = Convert.ToBoolean(await settingManager.GetOrNullGlobalAsync(BaseConfiguraciones.Identidad.ControlarClavesAnterior));

            logger.LogDebug("Validar contraseñas anteriores del usuario {usuario}. Aplicar control {controlarClavesAnterior}", eventData.Entity.UserName, controlarClavesAnterior);

            if (controlarClavesAnterior && eventData.Entity.PasswordHash != null)
            {
                using (var uow = unitOfWorkManager.Begin())
                {
                    try
                    {
                        var usuario = eventData.Entity;

                        var consulta = await repository.GetQueryableAsync();
                        var consultaValores = consulta.Where(a => a.UsuarioId == usuario.Id)
                                           .OrderByDescending(a => a.CreationTime)
                                           .Take(1)
                                           .Select(a => a.ClaveHash)
                                           ;

                        var ultimaClave = await asyncExecuter.SingleOrDefaultAsync(consultaValores);

                        //Si la ultima clave en el historico, asociado al usuario, no pasa la validacion hash, entonces registrar el hash actual usuario en 
                        //el historico
                        if (ultimaClave == null || userManager.PasswordHasher.VerifyHashedPassword(usuario, ultimaClave, usuario.PasswordHash) == PasswordVerificationResult.Failed)
                        {

                            var historico = new UsuarioHistorico(guidGenerator.Create(), usuarioId: usuario.Id, claveHash: usuario.PasswordHash);
                            await repository.InsertAsync(historico);

                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Registrar historico. Usuario {usuario}.", eventData.Entity.UserName);
                        throw;
                    }

                    await uow.CompleteAsync();
                }

            }
                
        }
    }
}
