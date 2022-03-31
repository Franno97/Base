using System;
using System.Linq;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mre.Sb.Base.EntityFrameworkCore;
using Mre.Sb.Base.Localization;
using Mre.Sb.Base.MultiTenancy;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Mre.Sb.Base.Cuenta;
using Microsoft.Extensions.Configuration;
using Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;
using Mre.Sb.Base.Pages.Shared.Components.Pie;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Kafka;
using Volo.Abp.Auditing;
using Mre.Sb.Auditar;
using Mre.Sb.Cache;
using Volo.Abp.IdentityModel;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mre.Sb.Base.Auditar;
using Mre.Sb.AuditoriaConf;
using Mre.Sb.AuditoriaConf.Application;
using Mre.Sb.AuditoriaConf.EntityFrameworkCore;

namespace Mre.Sb.Base
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(BaseEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreSerilogModule)
        )]
    [DependsOn(
        typeof(AbpIdentityModelModule))]

    [DependsOn(typeof(BaseApplicationModule))] 

    [DependsOn(typeof(AbpEventBusKafkaModule))]
    
    [DependsOn(typeof(BaseHttpApiModule))]

    [DependsOn(
        typeof(AuditoriaConfHttpApiModule),
        typeof(AuditoriaConfApplicationModule),
        typeof(AuditoriaConfEntityFrameworkModule)
    )]

    [DependsOn(
        typeof(BaseCacheModule)
        )]

    
    public class BaseIdentityServerModule : AbpModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            PreConfigure<AbpEventBusOptions>(options =>
            {
                options.EnabledErrorHandle = true;
                options.UseRetryStrategy(retryStrategyOptions =>
                {
                    retryStrategyOptions.IntervalMillisecond = configuration.GetValue<int>("EventosDistribuidos:IntervaloTiempo");
                    retryStrategyOptions.MaxRetryAttempts = configuration.GetValue<int>("EventosDistribuidos:NumeroReintentos");
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            context.Services.AddSameSiteCookiePolicy();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BaseResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );

                //options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español", "es"));
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
                 
            });
  

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    //options.FileSets.ReplaceEmbeddedByPhysical<BaseDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Mre.Sb.Base.Domain.Shared"));
                    //options.FileSets.ReplaceEmbeddedByPhysical<BaseDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Mre.Sb.Base.Domain"));
                });
            }

            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));

                options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
                options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
            });

            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.IsJobExecutionEnabled = false;
            });
              

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "Base-Protection-Keys");
            }

            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });


            var openIdConfiguracion = new OpenIdConfiguracion();
            configuration.GetSection("AutentificacionExterna:OpenId").Bind(openIdConfiguracion);

            context.Services.Configure<OpenIdConfiguracion>(
           configuration.GetSection("AutentificacionExterna:OpenId"));
             
        
            context.Services.AddAuthentication()
            .AddOpenIdConnect(openIdConfiguracion.ProveedorNombre, openIdConfiguracion.NombreVisualizar, options =>
             {
                 options.Authority = openIdConfiguracion.Autoridad;
                 options.ClientId = openIdConfiguracion.ClienteId;
                 options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                 options.CallbackPath = openIdConfiguracion.UrlRetorno;
                 options.ClientSecret = openIdConfiguracion.ClienteClave;
                 options.RequireHttpsMetadata = false;
                 options.SaveTokens = true;
                 options.GetClaimsFromUserInfoEndpoint = true;
                 
                 //Personalizar, segun la configuracion final.
                 options.Scope.Add("email");
                 options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, openIdConfiguracion.ClaimMapeoUsuario);
                 
             });

            Configure<AbpLayoutHookOptions>(options =>
            {
                options.Add(
                    LayoutHooks.Body.Last, 
                    typeof(PieViewComponent) 
                );
            });


            //Configure<AbpClaimsServiceOptions>(options =>
            //{
            //    options.RequestedClaims.AddRange(new[]{
            //         BaseConsts.Claims.Permiso
            //    });
            //});

            //Configure<TokenCleanupOptions>(options =>
            //{
            //    options.CleanupPeriod = 300000;  //5 minutos
            //});


            RemoteServicesExtensions.ConfigureHttpClient(context, configuration, hostingEnvironment);


            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabled = false; //Desactivar auditoria abp
            });

            
            context.Services.AgregarAuditoria(configuration);

            //Obtener configuracion auditoria directamente
            context.Services.Replace(ServiceDescriptor.Transient<IAuditarConfiguracionProveedor, AuditarDirectoConfiguracionProveedor>());

            
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseStaticFiles();

         
            app.UseRouting();
            app.UseCors();

            app.UseCookiePolicy();

            app.UseAuthentication();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            app.UsarAuditoria<BaseDbContext>();

            
        }


    }
}
