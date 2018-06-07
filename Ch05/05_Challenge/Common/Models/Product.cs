using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HPlusSports.Models
{
    public class Product
    {
        public long Id { get; set; }

        [Required]
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

        [Required]
        [Display(Name = "Last Updated By")]
        public DateTime LastUpdated { get; set; }

        [Required]
        [Display(Name = "Last Update Timestamp")]
        public string LastUpdatedUserId { get; set; }

        [NotMapped]
        public virtual Category Category { get; set; }

        public long? ThumbnailImageId { get; set; }
        public virtual Image ThumbnailImage { get; set; }

        public virtual ICollection<Image> Images { get; private set; }

        public Product()
        {
            Images = new List<Image>();
        }
    }
}