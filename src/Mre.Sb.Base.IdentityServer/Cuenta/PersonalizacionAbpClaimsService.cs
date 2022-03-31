//using IdentityServer4.Services;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.IdentityServer;

//namespace Mre.Sb.Base.Cuenta
//{
//    [Dependency(ReplaceServices = true)]
//    public class PersonalizacionClaimsService : AbpClaimsService
//    {
//        private static readonly string[] AdicionalesClaimNombres =
//        {
//            BaseConsts.Claims.Permiso,
//            BaseConsts.Claims.UnidadAdministrativa
//        };


//        public PersonalizacionClaimsService(IProfileService profile, 
//            ILogger<PersonalizacionClaimsService> logger,
//            IOptions<AbpClaimsServiceOptions> options) : base(profile, logger, options)
//        {
//        }

//        protected override IEnumerable<Claim> GetOptionalClaims(ClaimsPrincipal subject)
//        {
//            var adicionales = ObtenerAdicionalClaims(subject);
//            return base.GetOptionalClaims(subject)
//                .Union(adicionales);
//        }

//        protected virtual IEnumerable<Claim> ObtenerAdicionalClaims(ClaimsPrincipal subject)
//        {
//            foreach (var claimNombre in AdicionalesClaimNombres)
//            {
//                var claims = subject.FindAll(claimNombre);
//                if (claims != null)
//                {
//                    foreach (var claim in claims)
//                    {
//                        yield return claim;
//                    } 
//                }
//            } 
//        }
//    }
//}
