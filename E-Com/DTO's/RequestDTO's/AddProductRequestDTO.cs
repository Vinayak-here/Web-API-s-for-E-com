using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com.DTO_s.RequestDTO_s
{
    public class AddProductRequestDTO
    {
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int StockQuantitty { get; set; }
        public string Color { get; set; }
      
    }
}
