using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kanopy.Startup))]
namespace Kanopy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
