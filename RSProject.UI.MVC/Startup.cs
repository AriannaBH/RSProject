using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RSProject.UI.MVC.Startup))]
namespace RSProject.UI.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
