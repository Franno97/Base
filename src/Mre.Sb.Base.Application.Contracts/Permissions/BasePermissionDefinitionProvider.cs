using Mre.Sb.Base.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Reflection;

namespace Mre.Sb.Base.Permissions
{
    public class BasePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var grupoModulo = context.AddGroup(BasePermissions.GroupName);

            //var permisosGrupoModulo = grupoModulo.AddPermission("Nombre.Permiso", L("Localizacion.Permiso"));


            var identityGroup = context.GetGroup(IdentityPermissions.GroupName);
            var identidadConfiguracionPermission = identityGroup.AddPermission(BasePermissions.IdentidadConfiguracion.Default, L("Permission:IdentidadConfiguracion"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BaseResource>(name);
        }
    }
}
