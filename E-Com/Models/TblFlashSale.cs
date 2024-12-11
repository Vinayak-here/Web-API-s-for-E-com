using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com.Models
{
    public class TblFlashSale
    {
        [Key]
        public int FlashSaleId { get; set; }  // Primary key for the flash sale

        [ForeignKey("TblProduct")]
        public int ProductId { get; set; }  // Foreign key referencing TblProduct

        public int DiscountPercentage { get; set; }  // Discount percentage during the flash sale

        public DateTime StartDate { get; set; }  // Start date and time of the flash sale

        public DateTime EndDate { get; set; }  // End date and time of the flash sale

        public int MaxQuantity { get; set; }  // Maximum quantity available for the flash sale

        public int SoldQuantity { get; set; }  // Quantity sold in the flash sale

        public string Status { get; set; }  // Status of the flash sale (Active, Ended, etc.)

        public DateTime CreatedDate { get; set; }  // Date when the flash sale was created

        public DateTime UpdatedDate { get; set; }  // Date when the flash sale details were updated

        public bool IsActive { get; set; }  // Whether the flash sale is currently active

        // Navigation property to TblProduct
        public virtual TblProduct Product { get; set; }  // Navigation to the associated product for the flash sale
    }
}
