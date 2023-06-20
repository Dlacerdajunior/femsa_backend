using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FemsaEP.Startup))]
namespace FemsaEP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
