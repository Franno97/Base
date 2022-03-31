using Mre.Sb.Base.Localization;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Mre.Sb.Base.Settings
{
    public class BaseSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Establecer configuraciones (key/value)
            context.Add(
                new SettingDefinition(BaseConfiguraciones.Institucion.Direccion, "", L("Institucion:Direccion"), L("Institucion:Direccion"),
                isVisibleToClients:true)
            );

            context.Add(
               new SettingDefinition(BaseConfiguraciones.Accesos.NotificarAccesoFallido,
               false.ToString(), 
               L("Acceso:NotificarFillido"), 
               L("Acceso:NotificarFallido:Descripcion"),
               isVisibleToClients: false)
           );

            context.Add(
                           new SettingDefinition(BaseConfiguraciones.Identidad.ControlarClavesAnterior,
                           false.ToString(),
                           L("Identidad:ControlarClavesAnterior"),
                           L("Identidad:ControlarClavesAnterior:Descripcion"),
                           isVisibleToClients: true)
                       );


            context.Add(
                          new SettingDefinition(BaseConfiguraciones.Identidad.ControlarClavesAnteriorCantidad,
                          5.ToString(),
                          L("Identidad:ControlarClavesAnteriorCantidad"),
                          L("Identidad:ControlarClavesAnteriorCantidad:Descripcion"),
                          isVisibleToClients: true)
                      );


            //Personalizacion configuraciones de modulos abp
            var isUserNameUpdateEnabled = context.GetOrNull(IdentitySettingNames.User.IsUserNameUpdateEnabled);
            if (isUserNameUpdateEnabled != null)
            {
                isUserNameUpdateEnabled.DefaultValue = false.ToString();
            }

            var isSelfRegistrationEnabled = context.GetOrNull("Abp.Account.IsSelfRegistrationEnabled");
            if (isSelfRegistrationEnabled != null)
            {
                isSelfRegistrationEnabled.DefaultValue = false.ToString();
            }


        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BaseResource>(name);
        }
    }
}
