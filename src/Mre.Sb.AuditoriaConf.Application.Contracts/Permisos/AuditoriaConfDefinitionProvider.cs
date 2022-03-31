using Mre.Sb.AuditoriaConf.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Mre.Sb.AuditoriaConf.Permisos
{
    public class AuditoriaConfDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var grupoModulo = context.AddGroup(AuditoriaConfPermissions.GroupName, L("Permission:AuditoriaConfManagement"));
 
         
            var auditarPermission = grupoModulo.AddPermission(AuditoriaConfPermissions.Auditar.Default, L("Permission:AuditarManagement"));
            auditarPermission.AddChild(AuditoriaConfPermissions.Auditar.Change, L("Permission:Change")); 
            auditarPermission.AddChild(AuditoriaConfPermissions.Auditar.Delete, L("Permission:Delete"));

 
            var auditablePermission = grupoModulo.AddPermission(AuditoriaConfPermissions.Auditable.Default, L("Permission:AuditableManagement"));
     

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AuditoriaConfResource>(name);
        }
    }
}
