using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GlobalProductCorp.Startup))]
namespace GlobalProductCorp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
