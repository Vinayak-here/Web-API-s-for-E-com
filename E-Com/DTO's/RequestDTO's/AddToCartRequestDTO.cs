using Microsoft.AspNetCore.SignalR;

namespace E_Com.DTO_s.RequestDTO_s
{
    public class AddToCartRequestDTO
    {
        public int UserId {  get; set; }    
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CartId {  get; set; }
    }
}
