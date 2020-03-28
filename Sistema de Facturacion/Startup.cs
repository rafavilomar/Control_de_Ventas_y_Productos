using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sistema_de_Facturacion.Startup))]
namespace Sistema_de_Facturacion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
