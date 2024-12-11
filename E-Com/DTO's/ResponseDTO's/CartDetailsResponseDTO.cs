using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com.DTO_s.ResponseDTO_s
{
    public class CartDetailsResponseDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int StockQuantitty { get; set; }
        public string Color { get; set; }
        public decimal TotalPrice {  get; set; }
    }
}
