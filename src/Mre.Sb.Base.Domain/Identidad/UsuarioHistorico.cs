using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mre.Sb.Base.Identidad
{
    public class UsuarioHistorico : CreationAuditedEntity<Guid>
    {

        public virtual Guid UsuarioId { get; protected set; }


        [DisableAuditing]
        public virtual string ClaveHash { get; protected set; }

        protected UsuarioHistorico()
        {

        }

        public UsuarioHistorico(
            Guid id,
            Guid usuarioId,
            [NotNull] string claveHash) : base(id)
        { 
            UsuarioId = usuarioId;
            ClaveHash = claveHash; 
        }
    }
}
