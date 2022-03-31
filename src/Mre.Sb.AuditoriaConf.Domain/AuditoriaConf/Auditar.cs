using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class Auditar : AuditedEntity<Guid> {

        protected Auditar()
        {
            Acciones = new List<string>();
        }

        public Auditar(Guid id, Guid auditableId, ICollection<string> acciones)
        {
            this.Id = id;
            this.AuditableId = auditableId;
            this.Acciones = acciones; 
        }

        [Required]
        public Guid AuditableId { get; set; }

     
        public Auditable Auditable { get; set; }

        public ICollection<string> Acciones { get; set; }

    }

}
