using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Volo.Abp.Identity
{
    public enum UserType
    {
        [Description("Interno")]
        Internal =1,

        [Description("Externo")]
        External =2
    }
}
