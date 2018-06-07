using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HPlusSports.Models
{
    public class ShoppingCart
    {
        public long Id { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Currency)]
        public double Tax { get; set; }

        [DataType(DataType.Currency)]
        public double Shipping { get; set; }

        [DataType(DataType.Currency)]
        public double Total
        {
            get { return Subtotal + Tax + Shipping; }
        }

        public string Coupon { get; set; }

        public virtual ICollection<ShoppingCartItem> Items { get; private set; }

        [DataType(DataType.Currency)]
        public double Subtotal
        {
            get { return Items.Sum(x => x.Total); }
        }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public void Recalculate()
        {
            Shipping = Subtotal * 0.1;
            Tax = Subtotal * 0.07;
        }
    }
}