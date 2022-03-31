using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mre.Sb.Base.Identidad
{
    public interface IPoliticaAutorizacionAppService
    { 
        Task<Dictionary<string, bool>> ObtenerListaAsync();

    }
     
}
