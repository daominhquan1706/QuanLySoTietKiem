using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLSTK.Startup))]
namespace QLSTK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
