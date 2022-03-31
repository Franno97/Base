using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mre.Sb.Notificacion.HttpApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace Mre.Sb.Base
{

    public static class RemoteServicesExtensions
    {

        public static void ConfigureHttpClient(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {


            //Configurar API externas
            var urlServicioNotificacion = configuration["RemoteServices:Notificacion:BaseUrl"];

            context.Services.AddHttpClient<INotificadorClient, NotificadorClient>(
                c =>
                {
                    c.BaseAddress = new Uri(urlServicioNotificacion);
                }) 
              ;
        }

    }

}
