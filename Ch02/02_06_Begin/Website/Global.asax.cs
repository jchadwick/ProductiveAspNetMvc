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
        public static ShoppingCart Cart
        {
            get
            {
                var context = new HPlusSportsDbContext();
                var userId = HttpContext.Current.User.Identity.Name;
                var cart = context.ShoppingCarts.FirstOrDefault(x => x.UserId == userId)
                           ?? new ShoppingCart { UserId = userId };
                return cart;
            }
        }

        protected void Application_Start()
        {
            DatabaseConfig.Initialize();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
