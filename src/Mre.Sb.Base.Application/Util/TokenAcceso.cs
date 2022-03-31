using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Mre.Sb.Base.Localization;
using Mre.Sb.Notificacion.HttpApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Tracing;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Mre.Sb.Base.Util
{
    public interface ITokenAcceso
    {

        Task<string> ObtenerAsync();

    }


    public class TokenAcceso : ITokenAcceso, ITransientDependency
    {
        protected IHttpContextAccessor HttpContextAccessor { get; }

        private readonly string BearerPrefix = "Bearer ";

        private readonly string CabeceraAutentificacion = "Authorization";

        public TokenAcceso(
            IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<string> ObtenerAsync()
        {
            if (HttpContextAccessor.HttpContext?.Request?.Headers == null)
            {
                return string.Empty;
            }

            var tokenAcceso = string.Empty;

            string autentificacionCabecera = HttpContextAccessor.HttpContext.Request.Headers[CabeceraAutentificacion];

            if (autentificacionCabecera != null && autentificacionCabecera.StartsWith(BearerPrefix)
                && !string.IsNullOrEmpty(autentificacionCabecera))
            {
                tokenAcceso = autentificacionCabecera.Substring(BearerPrefix.Length);
            }

            return await Task.FromResult(tokenAcceso);
        }
    }
}
