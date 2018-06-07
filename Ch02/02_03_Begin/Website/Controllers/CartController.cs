using HPlusSports.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace HPlusSports.Controllers
{
    public class CartController : Controller
    {
        private readonly HPlusSportsDbContext _context;

        public CartController()
            : this(new HPlusSportsDbContext())
        {
        }

        public CartController(HPlusSportsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var cart = GetCart();

            if (cart.Items.Any())
                return View("Cart", cart);

            return View("EmptyCart");
        }

        public ActionResult Add(string sku, int quantity = 1)
        {
            var product = _context.Products.FirstOrDefault(x => x.SKU == sku);

            var cart = GetCart();

            var item = cart.Items.FirstOrDefault(x => x.SKU == sku);

            if (item == null)
            {
                item = new ShoppingCartItem
                {
                    SKU = product?.SKU,
                    Name = product?.Name,
                    MSRP = (product?.MSRP).GetValueOrDefault(),
                    Price = (product?.Price).GetValueOrDefault(),
                    Quantity = quantity,
                };

                cart.Items.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            if (item.Quantity <= 0)
            {
                cart.Items.Remove(item);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors);

                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            TempData["SuccessMessage"] = $"Successfully added {item.Name} to the cart";

            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Remove(long id)
        {
            var cart = GetCart();

            var item = cart.Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                ModelState.AddModelError("", "Missing or invalid cart item");
                return View();
            }

            cart.Items.Remove(item);

            _context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }

        private ShoppingCart GetCart()
        {
            var userId = GetUserId(this);

            var cart =
                _context.ShoppingCarts
                    .Include("Items")
                    .FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(cart);
                _context.SaveChanges();
            }

            cart.Recalculate();

            return cart;
        }

        // Overwriteable function for unit testing
        internal Func<Controller, string> GetUserId =
            (controller) => controller.User.Identity.Name;
    }
}