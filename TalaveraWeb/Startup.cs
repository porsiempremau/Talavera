using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TalaveraWeb.Startup))]
namespace TalaveraWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
