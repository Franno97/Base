using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mre.Sb.Base.Localization;
using System;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Http;
using Volo.Abp.Localization.ExceptionHandling;

namespace Mre.Sb.ExceptionHandling
{
    public class PersonalizadoDefaultExceptionToErrorInfoConverter :
        DefaultExceptionToErrorInfoConverter, ITransientDependency
    {
        private const int ExceptionNumberReferenceConstraint = 547;
        private readonly IStringLocalizer<BaseResource> stringLocalizerLocal;
        private readonly ILogger<PersonalizadoDefaultExceptionToErrorInfoConverter> logger;

        public PersonalizadoDefaultExceptionToErrorInfoConverter(
            IOptions<AbpExceptionLocalizationOptions> localizationOptions, 
            IStringLocalizerFactory stringLocalizerFactory, 
            IStringLocalizer<AbpExceptionHandlingResource> stringLocalizer,
            IServiceProvider serviceProvider,
            IStringLocalizer<BaseResource> stringLocalizerLocal, 
            ILogger<PersonalizadoDefaultExceptionToErrorInfoConverter> logger)
            : base(localizationOptions, stringLocalizerFactory, stringLocalizer, serviceProvider)
        {
            this.stringLocalizerLocal = stringLocalizerLocal;
            this.logger = logger;
        }

        protected override RemoteServiceErrorInfo CreateErrorInfoWithoutCode(Exception exception, bool includeSensitiveDetails)
        {
         
            if (exception is DbUpdateException dbUpdateException
                && dbUpdateException.InnerException != null
                && dbUpdateException.InnerException is SqlException sqlException 
                && sqlException.Number == ExceptionNumberReferenceConstraint)
            {

                logger.LogError(sqlException, stringLocalizerLocal["Errores:IntegridadReferencial"]);

                return new RemoteServiceErrorInfo(stringLocalizerLocal["Errores:IntegridadReferencial"]);
            }

            return base.CreateErrorInfoWithoutCode(exception, includeSensitiveDetails);
        }

         
    }
}
