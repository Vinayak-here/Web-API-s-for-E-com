using E_Com.DTO_s.RequestDTO_s;
using E_Com.DTO_s.ResponseDTO_s;

namespace E_Com.Service.IService
{
    public interface ISellerService
    {
        public Task<ServiceResopnses<String>> SellerRegister(SellerRegistrationRequestDTO sellerRegistrationDTO);

        public  Task<ServiceResopnses<List<ProductDetailsResponseDTO>>> GetProductByCategory(int categoryid);

        public  Task<ServiceResopnses<string>> AddProduct(AddProductRequestDTO addProductRequestDTO);

        public  Task<ServiceResopnses<ProductDetailsResponseDTO>> GetProductById(int id);

        public Task<ServiceResopnses<SellerDetailsResponseDTO>> GetSellerByProductId(int id);

        public Task<ServiceResopnses<ProductDetailsAndSellerDetailsResponseDTO>> GetProductDetailsWithSellerDetails(int id);

        public Task<ServiceResopnses<string>>DeleteProductById(int id);

      


    }
}
