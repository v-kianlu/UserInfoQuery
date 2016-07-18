using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UserInfoQuery.Startup))]
namespace UserInfoQuery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
