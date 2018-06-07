using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPlusSports
{
    public class FeatureFoldersViewEngine : RazorViewEngine
    {
        public FeatureFoldersViewEngine()
        {
            var viewLocations = new [] {
                "~/Features/{1}/{0}.cshtml",
                "~/Features/{1}/Views/{0}.cshtml",
                "~/Features/Shared/{0}.cshtml",
            };

            MasterLocationFormats = viewLocations;
            PartialViewLocationFormats = viewLocations;
            ViewLocationFormats = viewLocations;
        }
    }
}