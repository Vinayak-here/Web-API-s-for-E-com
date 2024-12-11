namespace E_Com.DTO_s.ResponseDTO_s
{
    public class CartDetailsByCartIdResponseDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal ? ProductPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
