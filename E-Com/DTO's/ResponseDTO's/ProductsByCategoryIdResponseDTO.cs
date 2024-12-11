using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace E_Com.DTO_s.ResponseDTO_s
{
    public class ProductsByCategoryIdResponseDTO
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int StockQunatity { get; set; }
        public string CategoryName { get; set; }
    }
}
