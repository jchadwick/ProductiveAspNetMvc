using System.Web;
using System.Web.Mvc;
using HPlusSports.Extensions;

namespace HPlusSports
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AllowPartialRenderingAttribute());
        }
    }
}
