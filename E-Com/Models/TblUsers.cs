using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Com.Models
{
    public class TblUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("TblSeller")]
        public int SellerId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phonenumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Otp { get; set; } = string.Empty;

        public DateTime CreateedDate { get; set; }

        public bool isVerified { get; set; } = false;

        [ForeignKey("TblCart")]
        public int CartId { get; set; }

        // Navigation Properties
        public virtual TblCart Cart { get; set; }

        public virtual TblSeller Seller { get; set; }

        // Added relationship for reviews
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
