using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureAdsWebRole.Startup))]
namespace AzureAdsWebRole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
