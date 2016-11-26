using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(pbx_web.Startup))]

namespace pbx_web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) { ConfigureAuth(app); }
    }
}
