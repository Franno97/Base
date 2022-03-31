using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mre.Sb.Base.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Validation;

namespace Mre.Sb.Base.Cuenta
{
    
    public class PersonalizacionLoginModel : LoginModel
    {
        
        protected LinkGenerator LinkGenerator { get; }

        public OpenIdConfiguracion OpenIdConfiguracion { get; }

        public IStringLocalizer<BaseResource> Localizer { get; }


        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public FaseAcceso Fase { get; set; } = FaseAcceso.VerificarUsuario;

          
        [BindProperty]
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Usuario { get; set; }

        [BindProperty(SupportsGet = true)]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string UsuarioVerificado { get; set; }


        protected IIdentityServerInteractionService Interaction { get; }
        protected IClientStore ClientStore { get; }
        protected IEventService IdentityServerEvents { get; }

        public PersonalizacionLoginModel(
         IAuthenticationSchemeProvider schemeProvider,
         IOptions<Volo.Abp.Account.Web.AbpAccountOptions> accountOptions,
         IOptions<IdentityOptions> identityOptions,
         IIdentityServerInteractionService interaction,
         IClientStore clientStore,
         IEventService identityServerEvents,
         LinkGenerator linkGenerator,
         IOptions<OpenIdConfiguracion> openIdOptions,
         IStringLocalizer<BaseResource> localizer)
            : base(schemeProvider, accountOptions,  identityOptions)
        {
            this.LinkGenerator = linkGenerator;
            OpenIdConfiguracion = openIdOptions.Value;
            Localizer = localizer;

            Interaction = interaction;
            ClientStore = clientStore;
            IdentityServerEvents = identityServerEvents;
        }

         
        public override async Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginInputModel();

            

            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
             

            if (context != null)
            {
                ShowCancelButton = true;

                LoginInput.UserNameOrEmailAddress = context.LoginHint;

                //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
                var tenant = context.Parameters[TenantResolverConsts.DefaultTenantKey];
                if (!string.IsNullOrEmpty(tenant))
                {
                    CurrentTenant.Change(Guid.Parse(tenant));
                    Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
                }
            }



            if (context?.IdP != null)
            {
                LoginInput.UserNameOrEmailAddress = context.LoginHint;
                ExternalProviders = new[] { new ExternalProviderModel { AuthenticationScheme = context.IdP } };
                return Page();
            }

            if (!string.IsNullOrEmpty(UsuarioVerificado))
            {
                LoginInput.UserNameOrEmailAddress = UsuarioVerificado;
                Usuario = UsuarioVerificado;
            }

            var providers = await GetExternalProviders();
            ExternalProviders = providers.ToList();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            if (context?.Client?.ClientId != null)
            {
                var client = await ClientStore.FindEnabledClientByIdAsync(context?.Client?.ClientId);
                if (client != null)
                {
                    EnableLocalLogin = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            if (IsExternalLoginOnly)
            {
                return await base.OnPostExternalLogin(providers.First().AuthenticationScheme);
            }

            return Page();
        }

        public override async Task<IActionResult> OnPostAsync(string action)
        {
            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (action == "Cancel")
            {
                if (context == null)
                {
                    return Redirect("~/");
                }

                await Interaction.GrantConsentAsync(context, new ConsentResponse()
                {
                    Error = AuthorizationError.AccessDenied
                });

                return Redirect(ReturnUrl);
            }

            await CheckLocalLoginAsync();

            ValidateModel();

            await IdentityOptions.SetAsync();

            ExternalProviders = await GetExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            await ReplaceEmailToUsernameOfInputIfNeeds();

            var result = await SignInManager.PasswordSignInAsync(
                LoginInput.UserNameOrEmailAddress,
                LoginInput.Password,
                LoginInput.RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = LoginInput.UserNameOrEmailAddress,
                ClientId = context?.Client?.ClientId
            });

            if (result.RequiresTwoFactor)
            {
                return await TwoFactorLoginResultAsync();
            }

            if (result.IsLockedOut)
            {
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
            }

            if (result.IsNotAllowed)
            {
                Alerts.Warning(L["LoginIsNotAllowed"]);
                return Page();
            }

            if (!result.Succeeded)
            {
                Alerts.Danger(L["InvalidUserNameOrPassword"]);
                return Page();
            }

            //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
            var user = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
                       await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);

            if (user.ShouldChangePasswordOnNextLogin)
            {

                var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                var urlCambiarClave = LinkGenerator.GetPathByPage("/Account/ResetPassword", null, new
                {
                    UserId = user.Id,
                    ResetToken = token,
                    ReturnUrl = ReturnUrl
                });

                return RedirectSafely(urlCambiarClave, ReturnUrlHash);
            }

            Debug.Assert(user != null, nameof(user) + " != null");
            await IdentityServerEvents.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName)); //TODO: Use user's name once implemented

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }

        public override async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "", string returnUrlHash = "", string remoteError = null)
        {
            
            if (remoteError != null)
            {
                Logger.LogWarning($"External login callback error: {remoteError}");
                return RedirectToPage("./Login");
            }

            await IdentityOptions.SetAsync();

            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                Logger.LogWarning("External login info is not available");
                return RedirectToPage("./Login");
            }

            var result = await SignInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            );

            if (!result.Succeeded)
            {
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                    Action = "Login" + result
                });
            }

            if (result.IsLockedOut)
            {
                Logger.LogWarning($"External login callback error: user is locked out!");
                throw new UserFriendlyException("Cannot proceed because user is locked out!");
            }

            if (result.IsNotAllowed)
            {
                Logger.LogWarning($"External login callback error: user is not allowed!");
                throw new UserFriendlyException("Cannot proceed because user is not allowed!");
            }

            if (result.Succeeded)
            {
                return RedirectSafely(returnUrl, returnUrlHash);
            }

            //TODO: Handle other cases for result!

            var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email);
            if (email.IsNullOrWhiteSpace())
            {
                return RedirectToPage("./Register", new
                {
                    IsExternalLogin = true,
                    ExternalLoginAuthSchema = loginInfo.LoginProvider,
                    ReturnUrl = returnUrl
                });
            }

            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToPage("./AccessDenied", new
                {
                    returnUrl = ReturnUrl,
                    returnUrlHash = ReturnUrlHash
                }); 
            }
             
           
            if (await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey) == null)
            {
                CheckIdentityErrors(await UserManager.AddLoginAsync(user, loginInfo));
            }
            

            await SignInManager.SignInAsync(user, false);

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = user.Name
            });

            return RedirectSafely(returnUrl, returnUrlHash);
        }

        public virtual async Task<IActionResult> OnPostVerificarUsuarioAsync()
        {
            if (string.IsNullOrEmpty(Usuario))
            {
                ModelState.AddModelError("Usuario", "El campo Usuario es requerido.");
                return Page();
            }
             
            var usuarioExistente = await UserManager.FindByNameAsync(Usuario);

            if (usuarioExistente == null) {
                Alerts.Danger(Localizer["Acceso:UsuarioInvalido"]);
                return Page();
            }

            if (usuarioExistente.IsExternal) {
                 
                var provider = OpenIdConfiguracion.ProveedorNombre;
                var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
                var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                properties.Items["scheme"] = provider;

                return await Task.FromResult(Challenge(properties, provider));
            }

            LoginInput = new LoginInputModel();
            LoginInput.UserNameOrEmailAddress = usuarioExistente.UserName;

            ExternalProviders = await GetExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);


            var urlCambiarClave = LinkGenerator.GetPathByPage("/Account/Login", null, new
            {
                Fase = FaseAcceso.Autentificar,
                UsuarioVerificado = usuarioExistente.UserName,
                ReturnUrl = ReturnUrl
            });

            return RedirectSafely(urlCambiarClave, ReturnUrlHash); 
        }

    }

    public enum FaseAcceso { 
        VerificarUsuario = 1,
        Autentificar = 2
    }
}
