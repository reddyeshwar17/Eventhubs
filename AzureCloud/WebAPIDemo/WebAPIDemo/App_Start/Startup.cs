using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(WebAPIDemo.App_Start.Startup))]

namespace WebAPIDemo.App_Start
{
    public class Startup
    {
        //public static OAuthAuthorizationServerOptions OuthOptions { get; private set; }
        //public static string PublicClientId { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            //https://stackoverflow.com/questions/21519226/example-of-using-asp-net-identity-2-0-usermanagerfactory-with-useoauthbearertoke
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //PublicClientId = "self";

            //enable cors origin request
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var provider = new OwinProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                //AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = provider
                //Provider = new ApplicationOAuthProvider(PublicClientId) not required now
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
        }
    }
}
