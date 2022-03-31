using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mre.Sb.Base.Localization;
using Mre.Sb.Notificacion.HttpApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.IdentityModel;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Tracing;
using Volo.Abp.UI.Navigation.Urls;

namespace Mre.Sb.Base.Cuenta
{
    public class PersonalizacionAccountAppService : AccountAppService
    {
        private readonly INotificadorClient notificadorClient;
        private readonly IIdentityModelAuthenticationService identityModelAuthenticationService;
        private readonly IAppUrlProvider appUrlProvider;
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly AbpCorrelationIdOptions abpCorrelationIdOptions;
        private readonly IStringLocalizer<BaseResource> stringLocalizer;
        private readonly ILogger<PersonalizacionAccountAppService> logger;

        public AbpIdentityClientOptions ClientOptions { get; }


        public PersonalizacionAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository, 
            IAccountEmailer accountEmailer, 
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions,
            INotificadorClient notificadorClient,
            IIdentityModelAuthenticationService identityModelAuthenticationService,
            IOptions<AbpIdentityClientOptions> abpIdentityClientOptions,
            IAppUrlProvider appUrlProvider,
            ICorrelationIdProvider correlationIdProvider,
            IOptions<AbpCorrelationIdOptions> abpCorrelationIdOptions,
            IStringLocalizer<BaseResource> stringLocalizer,
            ILogger<PersonalizacionAccountAppService> logger)
            : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            this.notificadorClient = notificadorClient;
            this.identityModelAuthenticationService = identityModelAuthenticationService;
            ClientOptions = abpIdentityClientOptions.Value;
            this.appUrlProvider = appUrlProvider;
            this.correlationIdProvider = correlationIdProvider;
            this.abpCorrelationIdOptions = abpCorrelationIdOptions.Value;
            this.stringLocalizer = stringLocalizer;
            this.logger = logger;
             
        }

        
        public override async Task ResetPasswordAsync(ResetPasswordDto input)
        {
            await IdentityOptions.SetAsync();

            var user = await UserManager.GetByIdAsync(input.UserId);
            (await UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();

            if (user.ShouldChangePasswordOnNextLogin) {
                
                user.ShouldChangePasswordOnNextLogin = false;

                (await UserManager.UpdateAsync(user)).CheckErrors();
            }

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.ChangePassword
            });
        }


        public override async Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
        {
            var user = await GetUserByEmail(input.Email);

            if (user.UserType != Volo.Abp.Identity.UserType.External)
            {

                throw new UserFriendlyException(stringLocalizer["Cuenta:ReseteoClaveNoPermitido", input.Email]);
            }


            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);


            await SendPasswordResetLinkAsync(user, resetToken, input.AppName, input.ReturnUrl, input.ReturnUrlHash);

        }

        public virtual async Task SendPasswordResetLinkAsync(
            Volo.Abp.Identity.IdentityUser user,
            string resetToken,
            string appName,
            string returnUrl = null,
            string returnUrlHash = null)
        {
            Debug.Assert(CurrentTenant.Id == user.TenantId, "This method can only work for current tenant!");

            
            var url = await appUrlProvider.GetResetPasswordUrlAsync(appName);

            //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
            var link = $"{url}?userId={user.Id}&{TenantResolverConsts.DefaultTenantKey}={user.TenantId}&resetToken={UrlEncoder.Default.Encode(resetToken)}";

            if (!returnUrl.IsNullOrEmpty())
            {
                link += "&returnUrl=" + NormalizeReturnUrl(returnUrl);
            }

            if (!returnUrlHash.IsNullOrEmpty())
            {
                link += "&returnUrlHash=" + returnUrlHash;
            }

            //Notificacion 
            logger.LogInformation("Generar notificacion. Recuperacion Clave. Usuario {usuario}",user.UserName);

            var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


            notificadorClient.SetAccessToken(token);
            notificadorClient.AddHeaders(abpCorrelationIdOptions.HttpHeaderName, correlationIdProvider.Get());

            var notificacionDto = MapeoNotificacionMensaje(user, link);

            logger.LogInformation("RegistroPersona - Enviar codigo de verificacion al correo electronico");
            var notificacionResultado = await notificadorClient.NotificadorAsync(notificacionDto);



        }

        protected virtual NotificacionMensajeDto MapeoNotificacionMensaje(Volo.Abp.Identity.IdentityUser user,string enlaceRecuperacion)
        {
            var salida = new NotificacionMensajeDto();

            salida.Codigo = BaseConsts.Notificaciones.UsuarioClaveRecuperacion;
            salida.Asunto = stringLocalizer["Cuenta:PasswordReset"];
            salida.Destinatarios = user.Email;
            salida.Model = new Dictionary<string, object>();
            salida.Model.Add("EnlaceRecuperacion", enlaceRecuperacion);

            return salida;
        }

        protected virtual string NormalizeReturnUrl(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                return returnUrl;
            }

            //Handling openid connect login
            if (returnUrl.StartsWith("/connect/authorize/callback", StringComparison.OrdinalIgnoreCase))
            {
                if (returnUrl.Contains("?"))
                {
                    var queryPart = returnUrl.Split('?')[1];
                    var queryParameters = queryPart.Split('&');
                    foreach (var queryParameter in queryParameters)
                    {
                        if (queryParameter.Contains("="))
                        {
                            var queryParam = queryParameter.Split('=');
                            if (queryParam[0] == "redirect_uri")
                            {
                                return HttpUtility.UrlDecode(queryParam[1]);
                            }
                        }
                    }
                }
            }

            return returnUrl;
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
