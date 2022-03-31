using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Mre.Sb.Base.Cuenta
{
    public class PersonalizacionForgotPasswordModel : ForgotPasswordModel
    {
         

        //public override async Task<IActionResult> OnPostAsync()
        //{
        //    try
        //    {
        //        await AccountAppService.SendPasswordResetCodeAsync(
        //            new SendPasswordResetCodeDto
        //            {
        //                Email = Email,
        //                AppName = "MVC", //TODO: Const!
        //                ReturnUrl = ReturnUrl,
        //                ReturnUrlHash = ReturnUrlHash
        //            }
        //        );
        //    }
        //    catch (UserFriendlyException e)
        //    {
        //        Alerts.Danger(GetLocalizeExceptionMessage(e));
        //        return Page();
        //    }


        //    return RedirectToPage(
        //        "./PasswordResetLinkSent",
        //        new
        //        {
        //            returnUrl = ReturnUrl,
        //            returnUrlHash = ReturnUrlHash
        //        });
        //}
    }
}
