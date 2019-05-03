using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CollaborativeBath.Startup))]

namespace CollaborativeBath
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}