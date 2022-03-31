using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Reflection;

namespace Mre.Sb.AuditoriaConf.Permisos
{
    public static class AuditoriaConfPermissions
    {
        public const string GroupName = "AuditoriaConf";
         
     


        public static class Auditar
        {
            public const string Default = GroupName + ".Auditar";
            public const string Change = Default + ".Change";
            public const string Delete = Default + ".Delete";
        }


        public static class Auditable
        {
            public const string Default = GroupName + ".Auditable";
        }


    }
}
