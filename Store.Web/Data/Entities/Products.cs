using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Store.Web.Data.Entities
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int ProductId { get; set; }

        [MaxLength(50, ErrorMessage = "The fied {0} must contain less than {1} characteres.")]
        [Required]
        public string ProductName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }

        [DisplayName("Is Active")]
        public bool Discontinued { get; set; }

        [DisplayName("Is Starred")]
        public bool IsStarred { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Suppliers Supplier { get; set; }

        [DisplayName("Product Images Number")]
        public int ProductImagesNumberroperty => ProductImages == null ? 0 : ProductImages.Count;

        [DisplayName("Images")]
        public string ImageFullPatch => ProductImages == null || ProductImages.Count == 0
           ? $"http://warlinsano.somee.com/img/noimageg.jpg"
           : ProductImages.FirstOrDefault().ImageFullPatch;

        public virtual ICollection<ProductImage> ProductImages { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }


    }
}
