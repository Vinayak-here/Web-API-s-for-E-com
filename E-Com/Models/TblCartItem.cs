using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com.Models
{
    public class TblCartItem
    {
        [Key]
        public int CartItemId {get; set;}

        [ForeignKey("TblCart")]
        public int CartId { get; set;}

        [ForeignKey("TblProduct")] 
        public int ProductId {  get; set;}
        
        public int Quantity {  get; set;}  
        public decimal TotalPrice {  get; set;}
        public virtual TblProduct Product { get; set; }
        public virtual TblCart Cart { get; set; }


    }
}
