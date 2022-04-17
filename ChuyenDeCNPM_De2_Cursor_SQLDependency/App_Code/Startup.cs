using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChuyenDeCNPM_De2_Cursor_SQLDependency.Startup))]
namespace ChuyenDeCNPM_De2_Cursor_SQLDependency
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
