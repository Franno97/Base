using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Mre.Sb.Base.Identidad
{


    public interface IUsuarioAppService
    {

        Task<IdentityUserDto> CrearAsync(UsuarioCrearDto input);

        Task<IReadOnlyList<IdentityUserDto>> ObtenerListaAsync(List<Guid> input);

    }

     
}
