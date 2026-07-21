using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Caso1.Startup))]

namespace Caso1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
