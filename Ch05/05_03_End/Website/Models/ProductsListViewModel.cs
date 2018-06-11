using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HPlusSports.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
    }

    public class ProductViewModel
    {
        public const double MinimumAdvertisableDiscount = 0.05;

        public string SKU { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public double MSRP { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:P0}")]
        public double? Discount
        {
            get { return (MSRP - Price) / MSRP; }
        }

        public bool HasAdvertisableDiscount
        {
            get { return Discount > MinimumAdvertisableDiscount; }
        }

        public ProductRating Rating { get; set; }
    }
}