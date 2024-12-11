using E_Com.Data;
using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Models;
using E_Com.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Com.Repository
{
    public class SellerRepository : ISellerRepository
    {

        private readonly EComDbContext _dbContext;

        public SellerRepository(EComDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddProduct(TblProduct product)
        {
            try
            {
                await _dbContext.TblProduct.AddAsync(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public async Task<bool> AddSeller(TblSeller seller)
        {
            try
            {
                await _dbContext.TblSeller.AddAsync(seller);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                // Log the inner exception
                var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                Console.WriteLine("Inner exception: " + innerException);
                throw new Exception("Database update error: " + innerException, dbEx);
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckIfProductExistsProduct(int id)
        {
            var exists = await _dbContext.TblProduct.FindAsync(id);
            return true;
        }

        public async Task<bool> CheckingCategoryById(int id)
        {
            var exists = await _dbContext.TblCategory.FindAsync(id);
            return exists != null;
        }


        public async Task<bool> DeletProductById(int id)
        {
            var done = await _dbContext.TblProduct.FindAsync(id);
            _dbContext.TblProduct.Remove(done);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<TblProduct>> GetProductByCategoryId(int categoryId)
        {

            List<TblProduct> products = await _dbContext.TblProduct
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
            return products;

        }

        public async Task<TblProduct> GetProductByID(int id)
        {
            var product = await _dbContext.TblProduct.FindAsync(id);
            return product;
        }

        public async Task<ProductDetailsAndSellerDetailsResponseDTO> GetProductDetailsByProductId(int id)
        {
            var sellerDetails = await (from product in _dbContext.TblProduct
                                       join seller in _dbContext.TblSeller
                                       on product.SellerId equals seller.SellerId
                                       where product.ProductId == id
                                       select new ProductDetailsAndSellerDetailsResponseDTO
                                       {
                                           ProductName = product.ProductName,
                                           ProductDescription = product.ProductDescription,
                                           ProductPrice = product.ProductPrice,
                                           SellerName = seller.SellerName,
                                           GSTnumber = seller.GSTnumber,
                                       }).FirstOrDefaultAsync();

            return sellerDetails;
        }

        public async Task<TblSeller> GetSellerByProductId(int id)
        {
            var sellerDetails = await (from product in _dbContext.TblProduct
                                       join seller in _dbContext.TblSeller
                                       on product.SellerId equals seller.SellerId
                                       where product.ProductId == id
                                       select seller).FirstOrDefaultAsync();
            return sellerDetails;
        }

       
    }
}
