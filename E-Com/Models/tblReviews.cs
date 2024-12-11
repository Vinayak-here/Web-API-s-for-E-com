using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace E_Com.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }  // Primary Key

        [ForeignKey("TblUser")]
        public int UserId { get; set; }  // Foreign key to TblUser

        [ForeignKey("TblProduct")]
        public int ProductId { get; set; }  // Foreign key to TblProduct

        [Range(1, 5)]  // Ensuring rating is between 1 and 5
        public int Rating { get; set; }

        [StringLength(255)]
        public string Title { get; set; }  // Title of the review

        public string Description { get; set; }  // Detailed review description

        public DateTime ReviewDate { get; set; } = DateTime.Now;  // Date of review creation

        // Navigation properties for related entities
        public virtual TblUsers User { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
