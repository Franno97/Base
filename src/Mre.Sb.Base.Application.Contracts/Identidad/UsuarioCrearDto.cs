using Volo.Abp.Identity;

namespace Mre.Sb.Base.Identidad
{
    public class UsuarioCrearDto : IdentityUserCreateOrUpdateDtoBase
    {
        public UsuarioCrearDto()
        {
            //ShouldChangePasswordOnNextLogin = true;
        }
         

        //public bool SendActivationEmail { get; set; }

        //public bool SetRandomPassword { get; set; }

        //public bool ShouldChangePasswordOnNextLogin { get; set; }

    }

     
}
