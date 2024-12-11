using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Models;

namespace E_Com.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task<TblUsers> LoginAsync(string username, string password);

        public Task<List<TblUsers>> RegistrationAsync(TblUsers users);

        //public Task<ServiceResponses<TblUsers>> AuthenticateUserasync(LoginDTO loginDTO);

        public Task<TblUsers> FindinguserAsync(string email);

        public Task<TblUsers> UpdatingUserAsync(TblUsers user);

        public Task<TblUsers> SellerRegistrationAPI(int id);
        public Task<bool> CheckIfUserExists(int id);

        public Task<bool> CheckIfCategoryExistsById(int id);
        public Task<bool> AddToCart(TblCartItem tblCartItem);
        public Task<TblUsers> FindinguserById(int id);


        public Task<bool> CheckIfCartExists(int id);

        public Task<string> GetCartDetailsByCartId(int cartId);


        public Task<string> GetProductsByCategoryId(int categoryId);


        public Task<List<FlashSalesProductsResponseDTO>> GetFlashSalesProducts();

    }

}
