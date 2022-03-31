using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class Categoria : Entity<string> {

        [Required]
        [StringLength(CategoriaConsts.MaxIdLongitud)]
        [RegularExpression(DominioComunesConsts.ExpresionRegular.UnicamenteNumberosYLetras)]
        public override string Id { get; protected set; }


        [Required]
        [StringLength(DominioComunesConsts.MaxNombreLongitud)]
        public virtual string Nombre { get; set; }


        protected Categoria()
        {

        }

        public Categoria(string id)
         : base(id)
        {

        }
    }
}
