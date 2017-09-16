using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BatchProxy.Startup))]

namespace BatchProxy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
