using HPlusSports.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPlusSports.Services
{
    public class ProductUpdateService
    {
        private readonly HPlusSportsDbContext _context;

        public ProductUpdateService(HPlusSportsDbContext context)
        {
            _context = context;
        }

        public UpdateProductResponse Update(UpdateProductRequest request)
        {
            var existing = _context.Products.Find(request.Id);

            if (existing == null)
            {
                return new UpdateProductResponse
                {
                    Success = false,
                    Message = $"Couldn't update product #\"{request.Id}\": product not found!",
                };
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

            return new UpdateProductResponse
            {
                Success = true,
                Message = $"Successfully updated \"{request.Name}\"",
            };
        }
    }
}
