using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class CategoriaDto : IEntityDto<string>
    {

        [Required]
        [StringLength(CategoriaConsts.MaxIdLongitud)]
        [RegularExpression(DominioComunesConsts.ExpresionRegular.UnicamenteNumberosYLetras)]
        public  string Id { get;  set; }


        [Required]
        [StringLength(DominioComunesConsts.MaxNombreLongitud)]
        public virtual string Nombre { get; set; }
    }
}
