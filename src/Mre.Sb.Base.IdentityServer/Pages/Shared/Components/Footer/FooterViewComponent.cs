using Microsoft.AspNetCore.Mvc;
using Mre.Sb.Base.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace Mre.Sb.Base.Pages.Shared.Components.Pie
{
    public class PieViewComponent : AbpViewComponent
    {
        public ISettingProvider SettingProvider { get; }

        public PieViewComponent(ISettingProvider  settingProvider)
        {
            SettingProvider = settingProvider;
        }


        public virtual async Task<IViewComponentResult> InvokeAsync()
        {
             
            var model = new PieModelView();
            model.InstitucionDireccion = await SettingProvider.GetOrNullAsync(BaseConfiguraciones.Institucion.Direccion);

            return await Task.FromResult<IViewComponentResult>(View("~/Pages/Shared/Components/Footer/Default.cshtml", model));
        }

         
    }

}
