using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;

namespace HPlusSports
{
    public static class HtmlHelperExtensions
    {

        public static IHtmlString Rating(this System.Web.Mvc.HtmlHelper html, Models.ProductRating rating)
        {
            if (rating == null || rating.ReviewCount == 0)
            {
                return new System.Web.Mvc.MvcHtmlString("<span><em>No Rating</em></span>");
            }

            return html.Partial("_Rating", rating);
        }

    }
}