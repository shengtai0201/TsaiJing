using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TsaiJing.WebApplication.Startup))]
namespace TsaiJing.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
