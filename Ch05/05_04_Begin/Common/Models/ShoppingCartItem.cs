using System.ComponentModel.DataAnnotations;

namespace HPlusSports.Models
{
    public class ShoppingCartItem
    {
        public long Id { get; set; }

        [Required]
        public string SKU { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(minimum: 0, maximum: double.MaxValue)]
        public double MSRP { get; set; }

        [DataType(DataType.Currency)]
        [Range(minimum: 0, maximum: double.MaxValue)]
        public double Price { get; set; }

        [Range(minimum: 0, maximum: int.MaxValue)]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public double Total
        {
            get { return Price * Quantity; }
        }

        public ShoppingCartItem()
        {
        }
    }
}