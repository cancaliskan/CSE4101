using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(araniyor.Startup))]

namespace araniyor
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR(); // gerekli sinalR methodu eklenerek signalR kütüphanesinin proje içerisinde kullanılacağını belirtiyoruz.

        }
    }
}
