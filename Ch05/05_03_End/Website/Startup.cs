using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HPlusSports.Startup))]
namespace HPlusSports
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
