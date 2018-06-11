using HPlusSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPlusSports.Controllers
{
    [RoutePrefix("home/{name}")]
    public class HomeController : Controller
    {
        private readonly HPlusSportsDbContext _context;

        public HomeController()
            : this(new HPlusSportsDbContext())
        {
        }

        public HomeController(HPlusSportsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var categories =
                (
                    from category in _context.Categories
                    let count = _context.Products.Count(x => x.CategoryId == category.Id)
                    select new { category, count }
                ).ToDictionary(x => x.category, x => x.count);

            return View(categories);
        }

        [Route("about")]
        public ActionResult About(string name)
        {
            ViewBag.Message = "Your application description page, " + name;

            return View();
        }

        public ActionResult Broken(string error)
        {
            throw new Exception(error);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}