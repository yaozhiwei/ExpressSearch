using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(ExpressSearch.Startup))]
namespace ExpressSearch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer<Models.ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<Models.ApplicationDbContext>());

            ConfigureAuth(app);
        }
    }
}
