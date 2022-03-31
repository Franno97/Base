using System.Collections.Generic;

namespace Mre.Sb.Base.IdentityServer
{
    public class IdentityServerConfiguration {

        public List<string> ApiResource { get; set; }

        public List<string> ApiScopes { get; set; }
         
        public Dictionary<string, IdentityServerClientConfiguration> Clients { get; set; }

    }


    public class IdentityServerClientConfiguration
    { 
    
        public string ClientId { get; set; }

        public string ClientSecret { get; set; } = "1q2w3e*";

        public string RootUrl { get; set; }

        public string[] RedirectUri { get; set; }

        public string[] CorsOrigin { get; set; } 
        
        /// <summary>
        /// Tiempo (Segundos) vida del token acceso
        /// </summary>
        public int AccessTokenLifetime { get; set; }

        /// <summary>
        /// Tiempo (segundos) vida del token refrescamiento
        /// </summary>
        public int AbsoluteRefreshTokenLifetime { get; set; }

        /// <summary>
        ///  "password", "client_credentials", "authorization_code"
        /// </summary>
        public string[] GrantTypes { get; set; }


        public string[] Scopes { get; set; }

        public bool RequirePkce { get; set; } = false;

    }
}
