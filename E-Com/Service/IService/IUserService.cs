using E_Com.DTO_s.RequestDTO_s;
using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Models;

namespace E_Com.Service.IService
{
    public interface IUserService
    {
        public Task<ServiceResopnses<string>> RegistrationAsync(RegistrationRequestDTO regDto);
        public Task<ServiceResopnses<string>> Loginasync(LoginRequestDTO loginDto);

        public Task<ServiceResopnses<string>> GenerateAndStoreOtpAsync(string email);

        public Task<ServiceResopnses<string>> OtpIsVerified(string email, string otp);

        public  Task<ServiceResopnses<string>> AddToCart(AddToCartRequestDTO addToCartDTO);
        public Task<ServiceResopnses<string>> UpdatePasswordAsync(string email, string password);

        public Task<ServiceResopnses<List<CartDetailsByCartIdResponseDTO>>> GetCartDetailsByCartId(int cartId);

        public Task<ServiceResopnses<List<ProductsByCategoryIdResponseDTO>>> GetProductsByCategoryID(int categoryId);

        public Task <ServiceResopnses<List<FlashSalesProductsResponseDTO>>> GetFlashSaleProducts();
    }
}
