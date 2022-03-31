using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace Mre.Sb.Base.IdentityServer
{
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IConfiguration _configuration;
        private readonly ICurrentTenant _currentTenant;

        public IdentityServerDataSeedContributor(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder,
            IConfiguration configuration,
            ICurrentTenant currentTenant)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
            _configuration = configuration;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {

                var identityServerConfiguration = new IdentityServerConfiguration();

                _configuration.GetSection("IdentityServer").Bind(identityServerConfiguration);


                await _identityResourceDataSeeder.CreateStandardResourcesAsync();
                await CreateApiResourcesAsync(identityServerConfiguration);
                await CreateApiScopesAsync(identityServerConfiguration);
                await CreateClientsAsync(identityServerConfiguration);
            }
        }

        private async Task CreateApiScopesAsync(IdentityServerConfiguration identityServerConfiguration)
        {

            foreach (var apiScope in identityServerConfiguration.ApiScopes)
            {
                await CreateApiScopeAsync(apiScope);

            }

        }

        private async Task CreateApiResourcesAsync(IdentityServerConfiguration identityServerConfiguration)
        {
            var commonApiUserClaims = new[]
            {
                "email",
                "email_verified",
                "name",
                "phone_number",
                "phone_number_verified",
                "role"
            };


            foreach (var apiResource in identityServerConfiguration.ApiResource)
            {
                await CreateApiResourceAsync(apiResource, commonApiUserClaims);
            } 
        }

        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task<ApiScope> CreateApiScopeAsync(string name)
        {
            var apiScope = await _apiScopeRepository.GetByNameAsync(name);
            if (apiScope == null)
            {
                apiScope = await _apiScopeRepository.InsertAsync(
                    new ApiScope(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            return apiScope;
        }

        private async Task CreateClientsAsync(IdentityServerConfiguration identityServerConfiguration)
        {
            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address"
            };


            foreach (var clientConfiguration in identityServerConfiguration.Clients)
            {
                var identityServerClient = clientConfiguration.Value;


                var webClientRootUrl = identityServerClient.RootUrl?.TrimEnd('/');

                await CreateClientAsync(
                    name: clientConfiguration.Key,
                    scopes: identityServerClient.Scopes !=null ?  commonScopes.Union(identityServerClient.Scopes): commonScopes,
                    grantTypes: identityServerClient.GrantTypes,
                    secret: (identityServerClient.ClientSecret).Sha256(),
                    requireClientSecret: false,
                    redirectUris: identityServerClient.RedirectUri ?? (webClientRootUrl !=null? new string[] { webClientRootUrl } :null ) ,
                    postLogoutRedirectUris: identityServerClient.RedirectUri ?? (webClientRootUrl != null ? new string[] { webClientRootUrl } : null),
                    corsOrigins: identityServerClient.CorsOrigin ?? (webClientRootUrl != null ? new string[] { webClientRootUrl.RemovePostFix("/") } : null),
                    accessTokenLifetime: identityServerClient.AccessTokenLifetime,
                    absoluteRefreshTokenLifetime: identityServerClient.AbsoluteRefreshTokenLifetime,
                    requirePkce: identityServerClient.RequirePkce
                );
            }
             

        }

        private async Task<Client> CreateClientAsync(
            string name,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret = null,
            string[] redirectUris = null,
            string[] postLogoutRedirectUris = null,
            string frontChannelLogoutUri = null,
            bool requireClientSecret = true,
            bool requirePkce = false,
            IEnumerable<string> permissions = null,
            IEnumerable<string> corsOrigins = null,
            int accessTokenLifetime = 3600,
            int absoluteRefreshTokenLifetime = 2592000)
        {
            var client = await _clientRepository.FindByClientIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client(
                        _guidGenerator.Create(),
                        name
                    )
                    {
                        ClientName = name,
                        ProtocolType = "oidc",
                        Description = name,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowOfflineAccess = true,
                        AbsoluteRefreshTokenLifetime = absoluteRefreshTokenLifetime, // 31536000, //365 days
                        AccessTokenLifetime = accessTokenLifetime, // 31536000, //365 days
                        AuthorizationCodeLifetime = 300,
                        IdentityTokenLifetime = 300,
                        RequireConsent = false,
                        FrontChannelLogoutUri = frontChannelLogoutUri,
                        RequireClientSecret = requireClientSecret,
                        RequirePkce = requirePkce
                    },
                    autoSave: true
                );
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (!secret.IsNullOrEmpty())
            {
                if (client.FindSecret(secret) == null)
                {
                    client.AddSecret(secret);
                }
            }

            if (redirectUris != null)
            {
                foreach (var redirectUri in redirectUris)
                {
                    if (client.FindRedirectUri(redirectUri) == null)
                    {
                        client.AddRedirectUri(redirectUri);
                    }
                }
               
            }

            if (postLogoutRedirectUris != null)
            {
                foreach (var postLogoutRedirectUri in postLogoutRedirectUris)
                {

                    if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                    {
                        client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                    }
                }

            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    name,
                    permissions,
                    null
                );
            }

            if (corsOrigins != null)
            {
                foreach (var origin in corsOrigins)
                {
                    if (!origin.IsNullOrWhiteSpace() && client.FindCorsOrigin(origin) == null)
                    {
                        client.AddCorsOrigin(origin);
                    }
                }
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}
