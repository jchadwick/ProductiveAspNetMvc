using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HPlusSports.Models;

namespace HPlusSports
{
    public static class UrlHelperExtensions
    {
        public static string Product(this System.Web.Mvc.UrlHelper urlHelper, Product product)
        {
            return Product(urlHelper, product?.SKU);
        }

        public static string Product(this System.Web.Mvc.UrlHelper urlHelper, string sku)
        {
            return urlHelper.Action("Product", "Products", new { id = sku });
        }
    }
}