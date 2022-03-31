
//using IdentityServer4.Models;
//using IdentityServer4.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Threading.Tasks;
//using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
//using Volo.Abp.Authorization;
//using Volo.Abp.Authorization.Permissions;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Security.Claims;

//namespace Mre.Sb.Base.Cuenta
//{

     
//    public class PermisoAgregarClaim : IAbpClaimsPrincipalContributor, ITransientDependency
//    {
//        private readonly IAbpAuthorizationPolicyProvider abpAutorizacionPoliticaProveedor;
//        private readonly IPermissionChecker permisoChequeo;
//        private readonly DefaultAuthorizationPolicyProvider defaultAutorizacionPoliticaProveedor;
//        private readonly IPermissionDefinitionManager definicionPoliticaGestor;
//        private readonly IAuthorizationService autorizacionServicio;

//        public PermisoAgregarClaim(IAbpAuthorizationPolicyProvider abpAutorizacionPolicaProveedor,
//            IPermissionChecker permisoChequeo,
//            DefaultAuthorizationPolicyProvider defaultAutorizacionPolicaProveedor,
//            IPermissionDefinitionManager definicionPoliticaGestor,
//            IAuthorizationService autorizacionServicio)
//        {
//            this.abpAutorizacionPoliticaProveedor = abpAutorizacionPolicaProveedor;
//            this.permisoChequeo = permisoChequeo;
//            this.defaultAutorizacionPoliticaProveedor = defaultAutorizacionPolicaProveedor;
//            this.definicionPoliticaGestor = definicionPoliticaGestor;
//            this.autorizacionServicio = autorizacionServicio;
//        }

//        public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
//        {
//            var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
//            var userId = identity?.FindUserId();
//            if (userId.HasValue)
//            {
//                var permisos = await GetPermisosOtorgadosAsync();
//                foreach (var permiso in permisos)
//                {
//                    identity.AddClaim(new Claim(BaseConsts.Claims.Permiso, permiso));
//                } 
//            }  
//        }

//        protected virtual async Task<ICollection<string>> GetPermisosOtorgadosAsync()
//        {
//            var permisos = new List<string>();

//            var politicaNombres = await abpAutorizacionPoliticaProveedor.GetPoliciesNamesAsync();
//            var abpPoliticaNombres = new List<string>();
//            var otraPoliticaNombres = new List<string>();


//            //Verificar politicas, con el mecanismo abp
//            foreach (var policyName in politicaNombres)
//            {
//                if (await defaultAutorizacionPoliticaProveedor.GetPolicyAsync(policyName) == null
//                    && definicionPoliticaGestor.GetOrNull(policyName) != null)
//                {
//                    abpPoliticaNombres.Add(policyName);
//                }
//                else
//                {
//                    otraPoliticaNombres.Add(policyName);
//                }
//            }

//            //Verificar otras politicas
//            foreach (var policyName in otraPoliticaNombres)
//            {

//                if (await autorizacionServicio.IsGrantedAsync(policyName))
//                {
//                    permisos.Add(policyName);
//                }
//            }

//            var result = await permisoChequeo.IsGrantedAsync(abpPoliticaNombres.ToArray());
//            foreach (var (key, value) in result.Result)
//            {
//                if (value == PermissionGrantResult.Granted)
//                {
//                    permisos.Add(key);
//                }
//            }

//            return permisos;
//        }
//    }
//}
