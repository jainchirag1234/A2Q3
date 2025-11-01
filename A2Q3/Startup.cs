using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(A2Q3.Startup))]
namespace A2Q3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
