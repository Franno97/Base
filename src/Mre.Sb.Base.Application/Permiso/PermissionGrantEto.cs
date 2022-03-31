using System;

namespace Mre.Sb.Base.Permiso
{
    public class PermissionGrantEto
    {
        public  string Name { get;  set; }

        public string ProviderName { get; set; }

        public string ProviderKey { get; set; }

        public  Guid? TenantId { get;  set; }
    }


}
