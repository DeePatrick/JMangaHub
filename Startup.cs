using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JMangaHub.Startup))]

namespace JMangaHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            ConfigureAuth(app);
        }
    }
}
