using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPlusSports.Extensions
{
    public class AllowPartialRenderingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var result = filterContext.Result as ViewResult;

            if (request == null)
                return;

            if (request.IsAjaxRequest())
            {
                filterContext.Result = new PartialViewResult
                {
                    TempData = result.TempData,
                    View = result.View,
                    ViewData = result.ViewData,
                    ViewEngineCollection = result.ViewEngineCollection,
                    ViewName = result.ViewName,
                };
            }
        }
    }
}