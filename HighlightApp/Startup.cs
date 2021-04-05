using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HighlightApp.Startup))]
namespace HighlightApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
