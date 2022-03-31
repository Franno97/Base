
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Mre.Sb.Base.Localization;
using Mre.Sb.Base.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Linq;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.Base.Identidad
{
    public class HistoricoPasswordValidator<TUser> : IPasswordValidator<TUser>
    where TUser : Volo.Abp.Identity.IdentityUser
    {
        private readonly IRepository<UsuarioHistorico, Guid> repository;
        private readonly IAsyncQueryableExecuter asyncExecuter;
        private readonly ISettingManager settingManager;
        private readonly IStringLocalizer<BaseResource> localizer;
        private readonly ILogger<HistoricoPasswordValidator<Volo.Abp.Identity.IdentityUser>> logger;

        public HistoricoPasswordValidator(IRepository<UsuarioHistorico, Guid> repository,
            IAsyncQueryableExecuter asyncExecuter,
            ISettingManager settingManager,
            IStringLocalizer<BaseResource> localizer,
            ILogger<HistoricoPasswordValidator<Volo.Abp.Identity.IdentityUser>> logger)
        {
            this.repository = repository;
            this.asyncExecuter = asyncExecuter;
            this.settingManager = settingManager;
            this.localizer = localizer;
            this.logger = logger;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var controlarClavesAnterior = Convert.ToBoolean(await settingManager.GetOrNullGlobalAsync(BaseConfiguraciones.Identidad.ControlarClavesAnterior));

            logger.LogDebug("Validar contraseñas anteriores del usuario {usuario}. Aplicar control {controlarClavesAnterior}", user.UserName, controlarClavesAnterior);

            if (controlarClavesAnterior && password != null) {

                var controlarClavesAnteriorCantidad = Convert.ToInt32(await settingManager.GetOrNullGlobalAsync(BaseConfiguraciones.Identidad.ControlarClavesAnteriorCantidad));
   

                var consulta = await repository.GetQueryableAsync();
                var consultaValores = consulta.Where(a => a.UsuarioId == user.Id)
                                   .OrderByDescending(a => a.CreationTime)
                                   .Take(controlarClavesAnteriorCantidad)
                                   .Select(up => up.ClaveHash)
                                   ;
  
                var clavesAnteriores = await asyncExecuter.ToListAsync(consultaValores);

                var existe = clavesAnteriores
                        .Any(clave => manager.PasswordHasher.VerifyHashedPassword(user, clave, password) != PasswordVerificationResult.Failed)
                        ;

                logger.LogDebug("Validar contraseñas anteriores del usuario {usuario}. Verificar claves anteriores: {controlarClavesAnteriorCantidad}. Resultado {resultadoVerificacionClavesAnteriores}", user.UserName, controlarClavesAnteriorCantidad, existe);


                if (existe)
                {
                    throw new UserFriendlyException(message: localizer["Identidad:ControlarClavesAnterior:NoPermitidoCambio", controlarClavesAnteriorCantidad]);
    
                }
            }

           
            return IdentityResult.Success;
        }
    }




}
