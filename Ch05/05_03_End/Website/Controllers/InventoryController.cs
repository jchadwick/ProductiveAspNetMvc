using System;
using System.Linq;
using System.Web.Mvc;
using HPlusSports.Models;
using HPlusSports.Requests;

namespace HPlusSports.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class InventoryController : Controller
    {
        private HPlusSportsDbContext _context;

        public InventoryController(HPlusSportsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var products =
              _context.Products
                .OrderBy(x => x.CategoryId)
                .ThenBy(x => x.Name);

            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var product = new Product
            {
                CategoryId = request.CategoryId,
                Description = request.Description,
                MSRP = request.MSRP,
                Name = request.Name,
                Price = request.Price,
                SKU = request.SKU,
                Summary = request.Summary,
                LastUpdated = DateTime.UtcNow,
                LastUpdatedUserId = GetUserId(this),
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData.SuccessMessage($"Successfully created \"{product.Name}\"");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Update(long id)
        {
            var existing = _context.Products.Find(id);

            if (existing == null)
            {
                TempData.ErrorMessage($"Couldn't update product #\"{id}\": product not found!");
            }

            return View(existing);
        }

        [HttpPost]
        public ActionResult Update(UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            request.LastUpdatedUserId = GetUserId(this);

            var existing = _context.Products.Find(request.Id);

            if (existing == null)
            {
                TempData.ErrorMessage($"Couldn't update product #\"{request.Id}\": product not found!");
                return View();
            }

            var hasPriceChanged = existing.Price != request.Price;

            existing.CategoryId = request.CategoryId;
            existing.Description = request.Description;
            existing.MSRP = request.MSRP;
            existing.Name = request.Name;
            existing.Price = request.Price;
            existing.SKU = request.SKU;
            existing.Summary = request.Summary;

            existing.LastUpdated = DateTime.UtcNow;

            _context.SaveChanges();

            if (hasPriceChanged)
            {
                var cartsToUpdate =
                  _context.ShoppingCarts
                    .Include("Items")
                    .Where(cart => cart.Items.Any(x => x.SKU == request.SKU));

                foreach (var cart in cartsToUpdate)
                {
                    foreach (var cartItem in cart.Items.Where(x => x.SKU == request.SKU))
                    {
                        cartItem.Price = request.Price;
                    }

                    cart.Recalculate();
                }
            }

            _context.SaveChanges();

            TempData.SuccessMessage($"Successfully updated \"{request.Name}\"");

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(long id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return ProductListError(
                  $"Couldn't delete \"{product.Name}\": product not found!"
                );
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            TempData["SuccessMessage"] =
              $"Successfully deleted \"{product.Name}\"";

            return RedirectToAction(nameof(Index));
        }

        private ActionResult ProductListError(string error)
        {
            TempData["ErrorMessage"] = error;
            return RedirectToAction(nameof(Index));
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewData["CategoryId"] =
              _context.Categories
                  .Select(x => new SelectListItem
                  {
                      Text = x.Name,
                      Value = x.Id.ToString(),
                  })
                  .ToArray();
        }

        // Overwriteable function for unit testing
        internal Func<Controller, string> GetUserId =
            (controller) => controller.User.Identity.Name;
    }
}
