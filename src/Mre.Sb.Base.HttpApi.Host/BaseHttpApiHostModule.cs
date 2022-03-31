using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mre.Sb.Base.EntityFrameworkCore;
using Mre.Sb.Base.MultiTenancy;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using Mre.Sb.Ldap;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Kafka;
using Mre.Sb.AuditoriaConf;
using Mre.Sb.AuditoriaConf.Application;
using Mre.Sb.AuditoriaConf.EntityFrameworkCore;
using Mre.Sb.Auditar;
using Volo.Abp.Auditing;
using Mre.Sb.Cache;
using Mre.Sb.Base.Auditar;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;


namespace Mre.Sb.Base
{
    [DependsOn(
        typeof(BaseHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(BaseApplicationModule),
        typeof(BaseEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule),
        typeof(LdapApplicationModule)

    )]

    [DependsOn(typeof(AbpEventBusKafkaModule))]
    [DependsOn(
        typeof(AuditoriaConfHttpApiModule),
        typeof(AuditoriaConfApplicationModule),
        typeof(AuditoriaConfEntityFrameworkModule)
    )]
    [DependsOn(
        typeof(BaseCacheModule)
        )]
    public class BaseHttpApiHostModule : AbpModule
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
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureSiteCookiePolicy(context, configuration);


            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration);
            ConfigureLocalization(); 
            ConfigureVirtualFileSystem(context);
            ConfigureRedis(context, configuration, hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);

            //TODO:
            //Configuracion duracion tokes generados. (*)
            //Tokens para confirmacion correo. (*)
            //Token para recuperar claves.
            //context.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
            //    opt.TokenLifespan = TimeSpan.FromDays(3));
            //}

          
            RemoteServicesExtensions.ConfigureHttpClient(context, configuration, hostingEnvironment);

            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabled = false; //Desactivar auditoria abp
            });

           
            context.Services.AgregarAuditoria(configuration);

            //Obtener configuracion auditoria directamente
            context.Services.Replace(ServiceDescriptor.Transient<IAuditarConfiguracionProveedor, AuditarDirectoConfiguracionProveedor>());

        }


        private void ConfigurarPublicacionEventos(ServiceConfigurationContext context, IConfiguration configuration)
        {

             
        }

        private void ConfigureSiteCookiePolicy(ServiceConfigurationContext context, IConfiguration configuration)
        {
         
            context.Services.AddSameSiteCookiePolicy();
        }


    

       

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    //options.FileSets.ReplaceEmbeddedByPhysical<BaseDomainSharedModule>(
                    //    Path.Combine(hostingEnvironment.ContentRootPath,
                    //        $"..{Path.DirectorySeparatorChar}Mre.Sb.Base.Domain.Shared"));
                    //options.FileSets.ReplaceEmbeddedByPhysical<BaseDomainModule>(
                    //    Path.Combine(hostingEnvironment.ContentRootPath,
                    //        $"..{Path.DirectorySeparatorChar}Mre.Sb.Base.Domain"));
                    //options.FileSets.ReplaceEmbeddedByPhysical<BaseApplicationContractsModule>(
                    //    Path.Combine(hostingEnvironment.ContentRootPath,
                    //        $"..{Path.DirectorySeparatorChar}Mre.Sb.Base.Application.Contracts"));
                    //options.FileSets.ReplaceEmbeddedByPhysical<BaseApplicationModule>(
                    //    Path.Combine(hostingEnvironment.ContentRootPath,
                    //        $"..{Path.DirectorySeparatorChar}Mre.Sb.Base.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            //Configure<AbpAspNetCoreMvcOptions>(options =>
            //{
            //    options.ConventionalControllers.Create(typeof(BaseApplicationModule).Assembly);
            //});
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = configuration["AuthServer:Audience"];
                });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            var scopes = configuration.GetSection("AuthServer:Scopes").GetChildren()
                  .ToDictionary(x => x.Key, x => x.Value);

            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                scopes,
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Base API", Version = "v1"});
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                //options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español", "es"));
            });
        }

        private void ConfigureRedis(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "Base-Protection-Keys");
            }
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
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

            app.UseAuthorization();

            //activacion Swagger
            var scopes = context.GetConfiguration().GetSection("AuthServer:Scopes").GetChildren()
                 .ToDictionary(x => x.Key, x => x.Value);
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Base API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes(scopes.Keys.ToArray());
            });

          
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();

           
            app.UsarAuditoria<BaseDbContext>();
        }
    }
}
