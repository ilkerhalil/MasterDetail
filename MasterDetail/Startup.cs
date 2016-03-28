using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MasterDetail.Startup))]
namespace MasterDetail
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //try
            //{
            //    var configuration = new Configuration();
            //    configuration.Seed();
            //}
            //catch (DbEntityValidationException dbEntityValidationException)
            //{
            //    foreach (var dbEntityValidationResult in dbEntityValidationException.EntityValidationErrors)
            //    {

            //    }
            //}

            ConfigureAuth(app);
        }
    }
}
