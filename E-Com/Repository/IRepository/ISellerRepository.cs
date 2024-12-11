using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Models;

namespace E_Com.Repository.IRepository
{
    public interface ISellerRepository
    {
        public Task<bool> AddSeller(TblSeller seller);
        public Task<bool> AddProduct(TblProduct product);

        public  Task<bool> CheckingCategoryById(int id);
        public Task<List<TblProduct>> GetProductByCategoryId(int categoryId);

        public Task<TblProduct> GetProductByID(int id);

        public Task<bool> CheckIfProductExistsProduct(int id);

        public Task<TblSeller> GetSellerByProductId(int id);

        public Task<bool> DeletProductById(int id);

        public Task<ProductDetailsAndSellerDetailsResponseDTO> GetProductDetailsByProductId(int id);

 


    }
}
