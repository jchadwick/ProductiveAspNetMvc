using System.Linq;
using System.Web.Mvc;
using HPlusSports.Models;

namespace HPlusSports.Controllers
{
    public class ProductsController : Controller
    {
        public const int DefaultPageSize = 8;

        private readonly HPlusSportsDbContext _context;

        public ProductsController()
            : this(new HPlusSportsDbContext())
        {
        }

        public ProductsController(HPlusSportsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index(int? page = null, int? count = null)
        {
            return Category(null, page, count);
        }

        public ActionResult Category(string id, int? page = null, int? count = null)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Key == id);

            IQueryable<Product> products = _context.Products;

            if (category == null)
            {
                products =
                  products
                    .OrderBy(x => x.CategoryId)
                    .ThenBy(x => x.Name);
            }
            else
            {
                products =
                  products
                    .Where(x => x.CategoryId == category.Id)
                    .OrderBy(x => x.Name);
            }

            var skus = products.Select(x => x.SKU);
            var ratings = _context.GetProductRatings(skus);

            ViewData["Ratings"] = ratings;
            ViewData["Category"] = category ?? new Category { Name = "All Categories" };

            /**** Paging Logic ****/
            var model = products;
            var resultsCount = model.Count();
            var pageSize = count.GetValueOrDefault(DefaultPageSize);
            var currentPage = page.GetValueOrDefault(1);
            var pageCount = resultsCount / pageSize + (resultsCount % pageSize > 0 ? 1 : 0);
            var previousPage = (currentPage - 1 > 0) ? currentPage - 1 : (int?)null;
            var nextPage = (currentPage + 1 <= pageCount) ? currentPage + 1 : (int?)null;

            model = model
              .Skip((currentPage - 1) * pageSize)
              .Take(pageSize);

            ViewData["PageSize"] = pageSize;
            ViewData["ResultsCount"] = resultsCount;
            ViewData["CurrentPage"] = currentPage;
            ViewData["PageCount"] = pageCount;
            ViewData["PreviousPage"] = previousPage;
            ViewData["NextPage"] = nextPage;

            /**** End Paging Logic ****/

            return View("ProductList", model);
        }

        public ActionResult Product(string id)
        {
            var product = _context.FindProductBySku(id);

            if (product == null)
                return HttpNotFound();

            var imageIds =
                product
                  .Images
                    .Select(img => img.Id)
                    .ToArray();

            ViewData["Rating"] = _context.GetProductRating(product.SKU);
            ViewData["Category"] = product.Category;
            ViewData["Images"] = imageIds;

            return View(product);
        }
    }
}