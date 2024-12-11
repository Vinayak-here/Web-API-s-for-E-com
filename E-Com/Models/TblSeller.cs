using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com.Models
{
    public class TblSeller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SellerId { get; set; }

        public int UserId { get; set; }

        public string SellerName { get; set; }

        public string GSTnumber { get; set; }


        public string LicenseNumber { get; set; }

        public string Address { get; set; }

        
        public virtual ICollection<TblProduct> Products { get; set; } = new List<TblProduct>();
        public virtual TblUsers User { get; set; }
    }
}
