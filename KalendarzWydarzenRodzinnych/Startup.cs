using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KalendarzWydarzenRodzinnych.Startup))]
namespace KalendarzWydarzenRodzinnych
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
