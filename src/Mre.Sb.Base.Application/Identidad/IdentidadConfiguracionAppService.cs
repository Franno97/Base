using Mre.Sb.Base.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Settings;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.Base.Identidad
{
    public class IdentidadConfiguracionAppService : ApplicationService, IIdentidadConfiguracionAppService
    {
        protected ISettingManager SettingManager { get; }

        public IdentidadConfiguracionAppService(ISettingManager settingManager)
        {
            this.SettingManager = settingManager;
        }

        public async Task ActualizarAsync(ActualizarIdentidadConfiguracionDtoDto input)
        {
            await SettingManager.SetGlobalAsync(IdentitySettingNames.Password.RequiredLength, input.ClaveLongitud.ToString());
            await SettingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireDigit, input.ClaveRequiereDigito.ToString());
            await SettingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireUppercase, input.ClaveRequiereMayusculas.ToString());
            await SettingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireLowercase, input.ClaveRequiereMinusculas.ToString());
            await SettingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, input.ClaveRequiereNoAlfanumericos.ToString());
            await SettingManager.SetGlobalAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, input.BloqueoMaximoAccesoFallidos.ToString());
            await SettingManager.SetGlobalAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, input.BloqueoNuevosUsuarios.ToString());

            await SettingManager.SetGlobalAsync(IdentitySettingNames.Lockout.LockoutDuration, input.BloqueoTiempo.ToString());

            await SettingManager.SetGlobalAsync(BaseConfiguraciones.Accesos.NotificarAccesoFallido, input.AccesoNotificarFallidos.ToString());
            
            await SettingManager.SetGlobalAsync(BaseConfiguraciones.Identidad.ControlarClavesAnterior, input.ControlarClavesAnterior.ToString());
            await SettingManager.SetGlobalAsync(BaseConfiguraciones.Identidad.ControlarClavesAnteriorCantidad, input.ControlarClavesAnteriorCantidad.ToString());


        }

        public async Task<IdentidadConfiguracionDto> ObtenerAsync()
        {
            return new IdentidadConfiguracionDto
            {
                ClaveLongitud = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Password.RequiredLength)),
                ClaveRequiereDigito = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Password.RequireDigit)),
                ClaveRequiereMayusculas = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Password.RequireUppercase)),
                ClaveRequiereMinusculas = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Password.RequireLowercase)),
                ClaveRequiereNoAlfanumericos = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Password.RequireNonAlphanumeric)),
                BloqueoMaximoAccesoFallidos = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts)),
                BloqueoNuevosUsuarios = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Lockout.AllowedForNewUsers)),
                //TODO: Trabajar con int, o con TimeSpan
                BloqueoTiempo = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(IdentitySettingNames.Lockout.LockoutDuration)),

                AccesoNotificarFallidos = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(BaseConfiguraciones.Accesos.NotificarAccesoFallido)),

                ControlarClavesAnterior = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(BaseConfiguraciones.Identidad.ControlarClavesAnterior)),
                ControlarClavesAnteriorCantidad = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(BaseConfiguraciones.Identidad.ControlarClavesAnteriorCantidad))
                 

         }; 
        }
 
       
    }

     
}
