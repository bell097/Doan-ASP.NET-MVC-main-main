using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Doan_ASP.NET_MVC.Startup))]
namespace Doan_ASP.NET_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
