using Owin;
using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using Authorization.Providers;

[assembly: OwinStartup(typeof(Authorization.Startup))]

namespace Authorization
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; set; }
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new OAuthCustomTokenProvider(), //create this method
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                //Only for development allow insecure http
                AllowInsecureHttp = true,
            };

            //Enabling your app to use auth bearer tokens
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
