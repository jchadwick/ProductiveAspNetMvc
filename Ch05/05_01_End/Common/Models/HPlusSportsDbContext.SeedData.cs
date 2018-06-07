using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using HPlusSports.Models;
using System.Text.RegularExpressions;

namespace HPlusSports
{
    public class HPlusSportsDbContextInitializer
        : DropCreateDatabaseIfModelChanges<HPlusSportsDbContext>
    {
        protected override void Seed(HPlusSportsDbContext context)
        {
            new SeedData().Populate(context);
        }
    }

    internal class SeedData
    {
        static readonly Random Random = new Random(1);

        public const string TestUserId = "demo@hplussports.com";

        static readonly IList<string> UserIds =
          Enumerable.Range(0, 10)
            .Select(x => $"user{x}")
            .Concat(new[] { TestUserId })
            .ToArray();

        public void Populate(HPlusSportsDbContext context)
        {
            var doc = ReadTestData();
            Populate(context, doc);
        }

        internal void Populate(HPlusSportsDbContext context, XDocument doc)
        {
            var categories = doc.Descendants("Categories").Descendants("Category")
              .Select(x => new Category
              {
                  Key = ToKey(x.Element("Name").Value),
                  Name = x.Element("Name").Value,
                  Image = new Image { Url = x.Element("ImageUrl").Value },
              })
              .ToArray();

            context.Images.AddRange(categories.Select(x => x.Image));
            context.SaveChanges();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var products = doc.Descendants("Product")
              .Select(x =>
              {
                  var product = new Product
                  {
                      CategoryId = categories.First(cat => cat.Name == x.Element("Category").Value).Id,
                      Name = x.Element("Name").Value,
                      Description = StripHtml(x.Element("Description").Value),
                      MSRP = double.Parse(x.Element("MSRP").Value),
                      Price = double.Parse(x.Element("Price").Value),
                      SKU = x.Element("SKU").Value,
                      Summary = x.Element("Summary").Value,
                      LastUpdated = DateTime.UtcNow,
                      LastUpdatedUserId = "admin@hplussports.com",
                      ThumbnailImage = new Image { Url = x.Element("ThumbnailImageUrl").Value },
                  };

                  product.Images.Add(new Image { Url = x.Element("ImageUrl").Value });

                  return product;
              })
              .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            var reviews = context.Products.SelectMany(GenerateReviews);
            context.Reviews.AddRange(reviews);
            context.SaveChanges();
        }

        IEnumerable<Review> GenerateReviews(Product product)
        {
            return Enumerable.Range(0, Random.Next(10))
              .Select(i =>
                new Review
                {
                    SKU = product.SKU,
                    UserId = UserIds[Random.Next(0, UserIds.Count)],
                    Rating = Random.Next(3, 5),
                });
        }

        private static string ToKey(string val)
        {
            return val
              .Replace(" ", "-")
              .Replace("--", "-")
              .Replace("--", "-")
              .ToLowerInvariant();
        }

        private static XDocument ReadTestData()
        {
            var set = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            using (var stream = typeof(SeedData).Assembly.GetManifestResourceStream("HPlusSports.TestData.xml"))
            using (var reader = new StreamReader(stream))
            {
                return XDocument.Parse(reader.ReadToEnd());
            }
        }

        private static string StripHtml(string source)
        {
            return
                Regex.Replace(
                    Regex.Replace(
                        source.Replace("<h2>Description</h2>", ""),
                        "<[^>]*>", 
                        ""
                    ),
                    "&#[^;]*;",
                    ""
                )
                .Trim();
        }
    }
}