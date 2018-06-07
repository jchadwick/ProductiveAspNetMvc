using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HPlusSports.App_Start;
using HPlusSports.Models;

namespace HPlusSports
{
    public class Application : HttpApplication
    {
        protected void Application_Start()
        {
            Dependencies.Register();
            DatabaseConfig.Initialize();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
