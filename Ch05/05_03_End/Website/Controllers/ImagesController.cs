using System.Linq;
using System.Web.Mvc;
using HPlusSports.Models;

namespace HPlusSports.Controllers
{
    public class ImagesController : Controller
    {
        private readonly HPlusSportsDbContext _context;

        public ImagesController()
            : this(new HPlusSportsDbContext())
        {
        }

        public ImagesController(HPlusSportsDbContext context)
        {
            _context = context;
        }

        // /images/category/{SKU}
        // Redirects to the main image for a category
        public ActionResult Category(string id)
        {
            var imageId =
                _context.Categories
                    .Where(x => x.Key == id)
                    .Select(x => x.ImageId)
                    .FirstOrDefault();

            return Image(imageId);
        }

        // /images/product/{SKU}
        // Redirects to the main image for a product
        public ActionResult Product(string id)
        {
            var imageId =
                _context.Products
                    .Where(x => x.SKU == id)
                    .SelectMany(x => x.Images.Select(img => img.Id))
                    .FirstOrDefault();

            return Image(imageId);
        }

        public ActionResult Image(long? id)
        {
            var image = _context.Images.FirstOrDefault(x => x.Id == id);

            if (!string.IsNullOrWhiteSpace(image.Url))
            {
                return RedirectPermanent(image.Url);
            }

            if (image?.Content == null)
            {
                image = GetPlaceholderImage();
            }

            return File(image.Content, image.ContentType);
        }


        internal static volatile byte[] PlaceholderImageContent;

        private Image GetPlaceholderImage()
        {
            if (PlaceholderImageContent == null)
            {
                var path = Server.MapPath("~/Content/placeholder.jpg");
                PlaceholderImageContent = System.IO.File.ReadAllBytes(path);
            }

            return new Image
            {
                Content = PlaceholderImageContent,
                ContentType = "image/jpeg"
            };
        }
    }
}