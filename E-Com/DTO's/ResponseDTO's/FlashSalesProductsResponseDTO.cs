namespace E_Com.DTO_s.ResponseDTO_s
{
    public class FlashSalesProductsResponseDTO
    {
        public int ProductId { get; set; }  // Correct as an integer

        public string ProductName { get; set; }  // Correct as a string

        public decimal ProductPrice { get; set; }  // Use decimal for currency values

        public string ImageUrl { get; set; }  // Correct as a string

        public int DiscountPercentage { get; set; }  // Correct as an integer

        public decimal AverageRating { get; set; }  // Use decimal for average ratings

        public int ReviewCount { get; set; }  // Correct as an integer
    }
}

