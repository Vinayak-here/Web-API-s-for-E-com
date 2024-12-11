using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Com.Models
{
    public class TblProduct
    {
        [Key]
        public int ProductId { get; set; }

        [ForeignKey("TblCategory")]
        public int CategoryId { get; set; }

        [ForeignKey("TblSeller")]
        public int SellerId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public int StockQuantitty { get; set; }

        public string Color { get; set; }

        public bool IsAvailable { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }


        // Navigation Properties
        public virtual ICollection<TblCartItem> CartItems { get; set; } = new List<TblCartItem>();

        public virtual TblCategory Category { get; set; }

        public virtual TblSeller Seller { get; set; }

        // Added relationship for reviews
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();


        public virtual TblFlashSale FlashSale { get; set; }  // Navigation property for the flash sale associated with this product
    }
}
