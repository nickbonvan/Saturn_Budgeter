using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saturn_Budgeter.Startup))]
namespace Saturn_Budgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
