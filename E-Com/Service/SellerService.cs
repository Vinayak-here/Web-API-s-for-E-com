using E_Com.DTO_s.RequestDTO_s;
using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Models;
using E_Com.Repository.IRepository;
using E_Com.Service.IService;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;

namespace E_Com.Service
{
    public class SellerService : ISellerService
    {

        private readonly ISellerRepository _sellerRepositary;
        private readonly IUserRepository _userRepositary;
        private readonly IValidator<SellerRegistrationRequestDTO> _sellerRgistrationValidator;
        private readonly IValidator<AddProductRequestDTO> _addProductValidator;

        public SellerService(IValidator<AddProductRequestDTO> addProductDTO,   IUserRepository userRepositary, IValidator<SellerRegistrationRequestDTO> sellerRgistrationValidator, ISellerRepository sellerRepositary)
        {
            _sellerRepositary = sellerRepositary;
            _sellerRgistrationValidator = sellerRgistrationValidator;
            _addProductValidator = addProductDTO;
            _userRepositary = userRepositary;
        }

        public async Task<ServiceResopnses<string>> AddProduct(AddProductRequestDTO addProductRequestDTO)
        {
            var validationResult = await _addProductValidator.ValidateAsync(addProductRequestDTO);
            var res = new ServiceResopnses<string>();
            if (!validationResult.IsValid)
            {
                res.Success = false;
                res.Data = null;
                res.Message = string.Join("; " , validationResult.Errors.Select(e => e.ErrorMessage));
                return res;
            }
            var prod = new TblProduct
            {
                CategoryId = addProductRequestDTO.CategoryId,
                SellerId = addProductRequestDTO.SellerId,
                ProductName = addProductRequestDTO.ProductName,
                ProductDescription = addProductRequestDTO.ProductDescription,
                ProductPrice = addProductRequestDTO.ProductPrice,
                StockQuantitty = addProductRequestDTO.StockQuantitty,
                Color = addProductRequestDTO.Color,
                CreatedDate = DateTime.Now,
                IsAvailable = true,
            };
            var done = await _sellerRepositary.AddProduct(prod);
            if(!done)
            {
                res.Success = false;
                res.Message = "Failed to add product";
                res.Data = null;
                return res;
            }
            res.Success = true;
            res.Message = "Product Successfully added";
            res.Data = null;
            return res;
        }

        

        public async Task<ServiceResopnses<string>> DeleteProductById(int id)
        {

            var prod = await _sellerRepositary.CheckIfProductExistsProduct(id);
            var res = new ServiceResopnses<string>();
            if (prod == false)
            {
                res.Success = false;
                res.Message = "The product with this id Doesn't Exists";
                res.Data = null;
                return res;
            }

            var deleted = await _sellerRepositary.DeletProductById(id);

            if (!deleted)
            {
                res.Success = false;
                res.Message = "Failed to delete the Product";
                res.Data = null;
                return res; 
            }
            res.Success = true;
            res.Message = "Product Sucessfully Deleted";
            res.Data = null;
            return res;

        }

     


        public async Task<ServiceResopnses<List<ProductDetailsResponseDTO>>> GetProductByCategory(int categoryid)
        {
            var res = new ServiceResopnses<List<ProductDetailsResponseDTO>>();

            var idIsAvailable = await _sellerRepositary.CheckingCategoryById(categoryid);
            if (!idIsAvailable)
            {
                res.Success = false;
                res.Message = "This Category Doesn't exist";
                res.Data = null; 
                return res;
            }

   
            List<TblProduct> products = await _sellerRepositary.GetProductByCategoryId(categoryid);


            if (products == null || products.Count == 0)
            {
                res.Success = false;
                res.Message = "No products found for this category.";
                res.Data = null; 
                return res;
            }


            /*var productDetails = new List<ProductDetailsResponseDTO>();
             

           
            foreach (var product in products)
            {
             
                var productDetail = new ProductDetailsResponseDTO
                {
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    StockQuantitty = product.StockQuantitty,
                    Color = product.Color
                };

                
                productDetails.Add(productDetail);
            }*/

          
            var productDetails = products.Select(product => new ProductDetailsResponseDTO
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                StockQuantitty = product.StockQuantitty,
                Color = product.Color
            }).ToList();

            

            res.Success = true;
            res.Message = "Products retrieved successfully.";
            res.Data = productDetails; 

            return res; 
        }

        public async Task<ServiceResopnses<ProductDetailsResponseDTO>> GetProductById(int id)
        {
            var product = await _sellerRepositary.GetProductByID(id);
            var res = new ServiceResopnses<ProductDetailsResponseDTO>();
            if (product == null )
            {
                res.Success = false;
                res.Message = "Product not found";
                res.Data = null;
                return res;
            }

            var productDetail = new ProductDetailsResponseDTO();
            {
                productDetail.ProductName = product.ProductName;
                productDetail.ProductDescription = product.ProductDescription;
                productDetail.ProductPrice = product.ProductPrice;
                productDetail.Color = product.Color;
                productDetail.StockQuantitty = product.StockQuantitty;
            }
            res.Success = true;
            res.Message = "Product is found";
            res.Data = productDetail;
            return res;
                
        }

        public async Task<ServiceResopnses<ProductDetailsAndSellerDetailsResponseDTO>> GetProductDetailsWithSellerDetails(int id)
        {
            var prod = await _sellerRepositary.CheckIfProductExistsProduct(id);
            var res = new ServiceResopnses<ProductDetailsAndSellerDetailsResponseDTO>();
            if(prod == false)
            {
                res.Success = false;
                res.Message = "The product with this id Doesn't Exist";
                res.Data = null;
                return res;
            }
            var prodAndSellerDetails = await _sellerRepositary.GetProductDetailsByProductId(id);
            res.Success = true;
            res.Message = "Product with seller Details Found";
            res.Data = prodAndSellerDetails;
            return res;
        }

        public async Task<ServiceResopnses<SellerDetailsResponseDTO>> GetSellerByProductId(int id)
        {
            var prod = await _sellerRepositary.GetProductByID (id);
            var res = new ServiceResopnses<SellerDetailsResponseDTO>();

            if(prod == null )
            {
               res.Success= false;
                res.Message = "Product with Id Doesn't exist";
                res.Data = null;
            }

            var sellerDetails = await _sellerRepositary.GetSellerByProductId(id);
           
            if(sellerDetails == null )
            {
                res.Success = false;
                res.Message = "Seller Details Not found";
                res.Data = null;
            }
            var sellerDetail = new SellerDetailsResponseDTO();
            {
                sellerDetail.SellerName = sellerDetails.SellerName;
                sellerDetail.LicenseNumber = sellerDetails.LicenseNumber;
                sellerDetail.Address = sellerDetails.Address;
                sellerDetail.GSTnumber = sellerDetails.GSTnumber;
            }
            
            res.Success = true;
            res.Message = "Details Found";
            res.Data = sellerDetail;
            return res;
        }

        public async Task<ServiceResopnses<string>> SellerRegister(SellerRegistrationRequestDTO sellerRegistrationDTO)
        {
            var validationResult = await _sellerRgistrationValidator.ValidateAsync(sellerRegistrationDTO);
            var res = new ServiceResopnses<string>();

            if (!validationResult.IsValid)
            {

                res.Success = false;
                res.Message = string.Join($"; ", validationResult.Errors.Select(e => e.ErrorMessage));
                res.Data = null;
                return res;
            }

            var user = await _userRepositary.SellerRegistrationAPI(sellerRegistrationDTO.UserId);

            if (user == null)
            {
                res.Success = false;
                res.Message = "Please register as User before registering as Seller";
                res.Data = null;
                return res;
            }

            var newseller = new TblSeller
            {
                UserId = sellerRegistrationDTO.UserId,
                SellerName = sellerRegistrationDTO.SellerName,
                GSTnumber = sellerRegistrationDTO.GSTnumber,
                LicenseNumber = sellerRegistrationDTO.LicenseNumber,
                Address = sellerRegistrationDTO.Address
            };

            var reg = await _sellerRepositary.AddSeller(newseller);
            if (reg == false)
            {
                res.Success = false;
                res.Message = "Failed to register as a seller";
                res.Data = null;
                return res;
            }

            user.SellerId = newseller.SellerId;
            var done = await _userRepositary.UpdatingUserAsync(user);
            if (done == null)
            {
                res.Success = false;
                res.Message = "Failed to update the UserTable with Seller ID";
                res.Data = null;
                return res;
            }
            res.Success = true;
            res.Message = "Seller registered successfully";
            res.Data = null;
            return res;
        }
    }
}

