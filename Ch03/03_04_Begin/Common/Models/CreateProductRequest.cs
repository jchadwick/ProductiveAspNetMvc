using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HPlusSports.Models
{
    public class CreateProductRequest
    {
        [Range(1, long.MaxValue)]
        public long CategoryId { get; set; }

        [Required]
        public string SKU { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Summary { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(minimum: 0, maximum: double.MaxValue)]
        [DataType(DataType.Currency)]
        public double MSRP { get; set; }

        [Range(minimum: 0, maximum: double.MaxValue)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
    }
}