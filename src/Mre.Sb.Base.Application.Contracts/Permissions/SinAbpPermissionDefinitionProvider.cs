using Mre.Sb.Base.Localization;
using Mre.Sb.Permiso;
using Mre.Visas.Tramite.Application.Contracts;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Mre.Sb.Base.Permissions
{
    /// <summary>
    /// Logica, para incluir definiciones de permisos, que se encuentra sin dependencias Abp
    /// </summary>
    public class SinAbpPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            ////Paso 1. Crear los objetos, que contiene la definicion grupos de permisos
            var tramitePermissionDefinitionProvider = new TramitePermissionDefinitionProvider();

            ////Paso 2. Agregar definiciones 
            AgregarPermisos(context, tramitePermissionDefinitionProvider);


            //Repetir proceso, para otros objetos que contienen definiciones de permisos Paso 1, Paso 2.

        }

        private static void AgregarPermisos(IPermissionDefinitionContext context, PermisoDefinicionProveedor permisoDefinicionProveedor)
        {
            var permisosGrupos = permisoDefinicionProveedor.Obtener();

            foreach (var permisoGrupo in permisosGrupos)
            {
                var grupoPermisoCreado = context.AddGroup(permisoGrupo.Nombre, L(permisoGrupo.Texto));

                foreach (var permiso in permisoGrupo.Permisos)
                {

                    var permisoPadreCreado = grupoPermisoCreado.AddPermission(permiso.Nombre, L(permiso.Texto));

                    foreach (var permisoHijo in permiso.Hijos)
                    {
                        permisoPadreCreado.AddChild(permisoHijo.Nombre, L(permisoHijo.Texto));

                    }
                }

            }
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BaseResource>(name);
        }
    }
}
