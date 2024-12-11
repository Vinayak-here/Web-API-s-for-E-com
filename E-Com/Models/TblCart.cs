using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com.Models
{
    public class TblCart
    {
        [Key]
        public int CartId { get; set; }
        public DateTime CreatedDate {  get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; }
        public virtual TblUsers User { get; set; }
        public virtual ICollection<TblCartItem> CartItems { get; set; }
    }
}
