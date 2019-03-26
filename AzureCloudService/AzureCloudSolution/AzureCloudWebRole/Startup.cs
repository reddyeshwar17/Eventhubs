using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureCloudWebRole.Startup))]
namespace AzureCloudWebRole
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
