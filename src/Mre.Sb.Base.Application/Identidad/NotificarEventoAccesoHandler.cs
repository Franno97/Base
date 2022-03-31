using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mre.Sb.Base.Localization;
using Mre.Sb.Base.Settings;
using Mre.Sb.Notificacion.HttpApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;
using Volo.Abp.IdentityModel;
using Volo.Abp.SettingManagement;
using Volo.Abp.Tracing;

namespace Mre.Sb.Base.Identidad
{

    public class NotificarEventoAccesoHandler
       : ILocalEventHandler<EntityChangedEventData<IdentitySecurityLog>>,
         ITransientDependency
    {
        private readonly IdentityUserManager gestionUsuario;
        private readonly INotificadorClient notificadorClient;
        private readonly IIdentityModelAuthenticationService identityModelAuthenticationService;
        private readonly ISettingManager settingManager;
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly AbpCorrelationIdOptions abpCorrelationIdOptions;
        private readonly IStringLocalizer<BaseResource> stringLocalizer;
        private readonly ILogger<NotificarEventoAccesoHandler> logger;

        private readonly string[] AccionesFallidas = new string[] { 
                IdentitySecurityLogActionConsts.LoginFailed,
                IdentitySecurityLogActionConsts.LoginInvalidUserName,
                IdentitySecurityLogActionConsts.LoginInvalidUserNameOrPassword,
                IdentitySecurityLogActionConsts.LoginLockedout,
                IdentitySecurityLogActionConsts.LoginNotAllowed
        };

        public  AbpIdentityClientOptions ClientOptions { get; }

        public NotificarEventoAccesoHandler(
            IdentityUserManager gestionUsuario,
            INotificadorClient notificadorClient,
            IIdentityModelAuthenticationService identityModelAuthenticationService,
            IOptions<AbpIdentityClientOptions> abpIdentityClientOptions,
            ISettingManager settingManager,
            ICorrelationIdProvider correlationIdProvider,
            IOptions<AbpCorrelationIdOptions> abpCorrelationIdOptions,
            IStringLocalizer<BaseResource> stringLocalizer,
            ILogger<NotificarEventoAccesoHandler> logger)
        {
            this.gestionUsuario = gestionUsuario;
            this.notificadorClient = notificadorClient;
            this.identityModelAuthenticationService = identityModelAuthenticationService;
            ClientOptions = abpIdentityClientOptions.Value;
            this.settingManager = settingManager;
            this.correlationIdProvider = correlationIdProvider;
            this.abpCorrelationIdOptions = abpCorrelationIdOptions.Value;
            this.stringLocalizer = stringLocalizer;
            this.logger = logger;
        }

        public async Task HandleEventAsync(EntityChangedEventData<IdentitySecurityLog> eventData)
        {

            await NotificarAsync(eventData.Entity);
        }

        private async Task<bool> NotificarAsync(IdentitySecurityLog logSeguridad) {

           
            //Se debe notificar.
            var notificarAccesosFallidos = Convert.ToBoolean(await settingManager.GetOrNullGlobalAsync(BaseConfiguraciones.Accesos.NotificarAccesoFallido));

            if (!notificarAccesosFallidos)
            {
                logger.LogDebug("No realizar notificaciones accesos fallidos. Accion {accesoAccion}", logSeguridad.Action);
                return false;
            }


            //Existe informacion usaurio para notificar
            if (!logSeguridad.UserId.HasValue) {
                logger.LogDebug("No existe informacion usuario. Accion {accesoAccion}",logSeguridad.Action);
                return false;
            }

            //Verificar que acciones, son accesos fallidos
            if (!AccionesFallidas.Contains(logSeguridad.Action)) {
                logger.LogDebug("La accion {accesoAccion}, no se encuentra configurada para notificar", logSeguridad.Action);
                return false;
            }

            logger.LogDebug("Realizar notificacion acceso. Accion {accesoAccion}. Usuario {usuario}", logSeguridad.Action,logSeguridad.UserName);

            //Notificacion 
            var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


            notificadorClient.SetAccessToken(token);
            notificadorClient.AddHeaders(abpCorrelationIdOptions.HttpHeaderName, correlationIdProvider.Get());

            var notificacionDto = await MapeoNotificacionMensajeAsync(logSeguridad);  
            var notificacionResultado = await notificadorClient.NotificadorAsync(notificacionDto);

            return notificacionResultado;
        }

        protected async Task<NotificacionMensajeDto> MapeoNotificacionMensajeAsync(IdentitySecurityLog logSeguridad)
        {

            //TODO: Obtener usuario actual o Obtener explicitamente informacion usuario..
            var usuario = await gestionUsuario.GetByIdAsync(logSeguridad.UserId.Value);

            var salida = new NotificacionMensajeDto();

            salida.Codigo = BaseConsts.Notificaciones.UsuarioAccesosFallidos;
            salida.Asunto = stringLocalizer["Acceso:NotificarFallido"];
            salida.Destinatarios = usuario.Email;
            salida.Model = new Dictionary<string, object>();
            salida.Model.Add("TipoAcceso", ConvertirTextoAccion(logSeguridad.Action));
            salida.Model.Add("Navegador", logSeguridad.BrowserInfo);
            salida.Model.Add("Aplicacion", logSeguridad.ApplicationName);
            salida.Model.Add("ClienteIp", logSeguridad.ClientIpAddress);
 
            return salida;
        }

        protected string ConvertirTextoAccion(string accion) {
             
            var tipoAccion = stringLocalizer["Acceso:TipoAccion:" + accion];

            return tipoAccion;
        }

        private IdentityClientConfiguration GetClientConfiguration(string identityClientName = null)
        {
            if (identityClientName.IsNullOrEmpty())
            {
                return ClientOptions.IdentityClients.Default;
            }

            return ClientOptions.IdentityClients.GetOrDefault(identityClientName) ??
                   ClientOptions.IdentityClients.Default;
        }
    }
}
