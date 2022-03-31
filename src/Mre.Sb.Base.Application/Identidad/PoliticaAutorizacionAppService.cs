using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;

namespace Mre.Sb.Base.Identidad
{
    public class PoliticaAutorizacionAppService : ApplicationService, IPoliticaAutorizacionAppService
    {
        private readonly IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider;
        private readonly DefaultAuthorizationPolicyProvider defaultAuthorizationPolicyProvider;
        private readonly IPermissionDefinitionManager permissionDefinitionManager;
        private readonly IPermissionChecker permissionChecker;
        private readonly IAuthorizationService authorizationService;

        public PoliticaAutorizacionAppService(IAbpAuthorizationPolicyProvider  abpAuthorizationPolicyProvider,
            DefaultAuthorizationPolicyProvider defaultAuthorizationPolicyProvider,
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionChecker permissionChecker,
            IAuthorizationService authorizationService)
        {
            this.abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            this.defaultAuthorizationPolicyProvider = defaultAuthorizationPolicyProvider;
            this.permissionDefinitionManager = permissionDefinitionManager;
            this.permissionChecker = permissionChecker;
            this.authorizationService = authorizationService;
        }

        public async Task<Dictionary<string, bool>> ObtenerListaAsync()
        {
            var resultado = new Dictionary<string, bool>();

            var politicaNombres = await abpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
            var abpPoliticaNombres = new List<string>();
            var otraPoliticaNombres = new List<string>();

            foreach (var policyName in politicaNombres)
            {
                if (await defaultAuthorizationPolicyProvider.GetPolicyAsync(policyName) == null
                    && permissionDefinitionManager.GetOrNull(policyName) != null)
                {
                    abpPoliticaNombres.Add(policyName);
                }
                else
                {
                    otraPoliticaNombres.Add(policyName);
                }
            }

            foreach (var politicaName in otraPoliticaNombres)
            {
              
                if (await authorizationService.IsGrantedAsync(politicaName))
                {
                    resultado.Add(politicaName,true);
                }
            }

            var result = await permissionChecker.IsGrantedAsync(abpPoliticaNombres.ToArray());
            foreach (var (key, value) in result.Result)
            {
                
                if (value == PermissionGrantResult.Granted)
                {
                    resultado.Add(key, true); 
                }
            }

            return resultado;
        }
    }
}
