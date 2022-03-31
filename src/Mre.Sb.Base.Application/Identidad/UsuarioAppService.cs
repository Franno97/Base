using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Mre.Sb.Base.Localization;
using Mre.Sb.Base.Util;
using Mre.Sb.Notificacion.HttpApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.IdentityModel;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Tracing;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Mre.Sb.Base.Identidad
{
    public class UsuarioAppService : ApplicationService, IUsuarioAppService
    {
        private readonly AbpCorrelationIdOptions abpCorrelationIdOptions;
        private readonly IIdentityModelAuthenticationService identityModelAuthenticationService;
 
        protected IdentityUserManager UserManager { get; }
        protected UserRoleFinder UserRoleFinder { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IRepository<IdentityUser, Guid> RepositoryDirect { get; }

        protected IOptions<IdentityOptions> IdentityOptions { get; }
        protected IEnumerable<IPasswordValidator<IdentityUser>> PasswordValidators { get; }
        
        protected INotificadorClient NotificadorClient { get; }
        public AbpIdentityClientOptions ClientOptions { get; }
        protected ICorrelationIdProvider CorrelationIdProvider { get; }
        protected IStringLocalizer<BaseResource> Localizer { get; }


        public UsuarioAppService(
            IdentityUserManager userManager,
            UserRoleFinder userRoleFinder,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IRepository<IdentityUser, Guid> repositoryDirect,
            IOptions<IdentityOptions> identityOptions,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            INotificadorClient notificadorClient,
            IIdentityModelAuthenticationService identityModelAuthenticationService,
            IOptions<AbpIdentityClientOptions> abpIdentityClientOptions,
            ICorrelationIdProvider correlationIdProvider,
            IOptions<AbpCorrelationIdOptions> abpCorrelationIdOptions,
            IStringLocalizer<BaseResource> localizer)
        {
            UserManager = userManager;
            UserRoleFinder = userRoleFinder;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            RepositoryDirect = repositoryDirect;
            IdentityOptions = identityOptions;
            PasswordValidators = passwordValidators;
            NotificadorClient = notificadorClient;
            this.identityModelAuthenticationService = identityModelAuthenticationService;
            ClientOptions = abpIdentityClientOptions.Value;
            CorrelationIdProvider = correlationIdProvider;
            this.abpCorrelationIdOptions = abpCorrelationIdOptions.Value;
            Localizer = localizer;
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public async Task<IdentityUserDto> CrearAsync(UsuarioCrearDto input)
        {
            await IdentityOptions.SetAsync();

            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            );

             
            //Custom
            user.UserType = input.UserType;
            user.Code = input.Code;

            input.MapExtraPropertiesTo(user);

            var password = string.Empty;
            if (input.UserType == Volo.Abp.Identity.UserType.External)
            {
                user.ShouldChangePasswordOnNextLogin = true;

                password = CreateRandomPassword();
                (await UserManager.CreateAsync(user, password)).CheckErrors();
            }
            else if (input.UserType == Volo.Abp.Identity.UserType.Internal)
            {
                //Autentificacion externa, para usuarios internos. 
                user.IsExternal = true; 

                (await UserManager.CreateAsync(user)).CheckErrors(); 
                
            }
            else
            {
                throw new NotSupportedException($"UserType Not Support {input.UserType}");
            }
             
            await UpdateUserByInput(user, input);
            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            //Notificacion
            var mensajeNotificacion = MapeoUsuarioModeloNotificacion(user, password);

            var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


            NotificadorClient.SetAccessToken(token);
            NotificadorClient.AddHeaders(abpCorrelationIdOptions.HttpHeaderName, CorrelationIdProvider.Get());

            await NotificadorClient.NotificadorAsync(mensajeNotificacion);

             
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        

        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<IReadOnlyList<IdentityUserDto>> ObtenerListaAsync(List<Guid> input)
        {

            var queryable = await RepositoryDirect.GetQueryableAsync();
            queryable = queryable.Where(a => input.Contains(a.Id));

            var list = await AsyncExecuter.ToListAsync(queryable);

            var listDto = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list);

            foreach (var item in listDto)
            {
                var roles = await UserRoleFinder.GetRolesAsync(item.Id);
                item.ExtraProperties.Add("Roles", roles);
            }

            return listDto;
        }

        #region Metodos Soporte

        protected virtual string CreateRandomPassword()
        {

            var upperCaseLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var lowerCaseLetters = "abcdefghijkmnopqrstuvwxyz";
            var digits = "0123456789";
            var nonAlphanumerics = "!@$?_-";

            string[] randomChars = {
                upperCaseLetters,
                lowerCaseLetters,
                digits,
                nonAlphanumerics
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (IdentityOptions.Value.Password.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    upperCaseLetters[rand.Next(0, upperCaseLetters.Length)]);
            }

            if (IdentityOptions.Value.Password.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    lowerCaseLetters[rand.Next(0, lowerCaseLetters.Length)]);
            }

            if (IdentityOptions.Value.Password.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    digits[rand.Next(0, digits.Length)]);
            }

            if (IdentityOptions.Value.Password.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    nonAlphanumerics[rand.Next(0, nonAlphanumerics.Length)]);
            }

            for (var i = chars.Count; i < IdentityOptions.Value.Password.RequiredLength; i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
         
        protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }

           (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;
            (await UserManager.UpdateAsync(user)).CheckErrors();

            user.SetIsActive(input.IsActive);

            if (input.RoleNames != null)
            {
                (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            }
        }

        private NotificacionMensajeDto MapeoUsuarioModeloNotificacion(IdentityUser user, string password)
        {
            var mensajeNotificacion = new NotificacionMensajeDto();

            mensajeNotificacion.Asunto = Localizer["IdentidadUsuario:NotificacionCreacion"];
            mensajeNotificacion.Destinatarios = user.Email;
            mensajeNotificacion.Model = new Dictionary<string, object>();
            mensajeNotificacion.Model.Add("Usuario", user.UserName);
            mensajeNotificacion.Model.Add("Email", user.Email);
            mensajeNotificacion.Model.Add("Nombres", user.Name);
            mensajeNotificacion.Model.Add("Apellidos", user.Surname);
            if (user.UserType == Volo.Abp.Identity.UserType.External)
            {
                mensajeNotificacion.Codigo = BaseConsts.Notificaciones.CreacionUsuarioExterno;
                mensajeNotificacion.Model.Add("Clave", password);
            }
            else if (user.UserType == Volo.Abp.Identity.UserType.Internal)
            {
                mensajeNotificacion.Codigo = BaseConsts.Notificaciones.CreacionUsuarioInterno;
            }

            return mensajeNotificacion;
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


        #endregion Metodos Soporte


    }


}
