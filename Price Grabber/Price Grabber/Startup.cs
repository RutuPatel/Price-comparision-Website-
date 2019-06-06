using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Price_Grabber.Startup))]
namespace Price_Grabber
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
