using E_Com.DTO_s.RequestDTO_s;
using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Models;
using E_Com.Repository;
using E_Com.Repository.IRepository;
using E_Com.Service.IService;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace E_Com.Service
{
    public class UserService : IUserService
    {
        private readonly ISellerRepository _sellerRepositary;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegistrationRequestDTO> _validator;

        public UserService(IUserRepository userRepository, IValidator<RegistrationRequestDTO> validator, IConfiguration configuration, ISellerRepository sellerRepositary)
        {
            _userRepository = userRepository;
            _validator = validator;
            _configuration = configuration;
            _sellerRepositary = sellerRepositary;
        }



        private string GenerateJwtToken(TblUsers user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<ServiceResopnses<string>> Loginasync(LoginRequestDTO loginDto)
        {
            var res = new ServiceResopnses<string>();

            var user = await _userRepository.LoginAsync(loginDto.Username, loginDto.Password);
            if (user == null)
            {
                res.Success = false;
                res.Message = "Username doesn't exist";
                res.Data = null;
                return res;
            }

            if (user.Password != loginDto.Password)
            {
                res.Success = false;
                res.Message = "Incorrect password";
                res.Data = null;
                return res;
            }

            var token = GenerateJwtToken(user);
            res.Success = true;
            res.Message = "Login successful";
            res.Data = $"{token}";
            return res;
        }

        public async Task<ServiceResopnses<string>> RegistrationAsync(RegistrationRequestDTO regDto)
        {
            var res = new ServiceResopnses<string>();

            var validationResult = await _validator.ValidateAsync(regDto);
            if (!validationResult.IsValid)
            {
                res.Success = false;
                res.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return res;
            }

            string username = GenerateUsername(regDto.FirstName, regDto.Phonenumber);

            var newUser = new TblUsers
            {
                Username = username,
                FirstName = regDto.FirstName,
                LastName = regDto.LastName,
                Phonenumber = regDto.Phonenumber,
                Email = regDto.Email,
                Password = regDto.Password,
                CreateedDate = DateTime.UtcNow
            };
            await _userRepository.RegistrationAsync(newUser);

            res.Success = true;
            res.Message = "User registered successfully";

            return res;
        }
        public string GenerateUsername(string firstName, string phoneNumber)
        {
            string firstPart = firstName.Length >= 4 ? firstName.Substring(0, 4) : firstName;
            string lastPart = phoneNumber.Length >= 4 ? phoneNumber.Substring(phoneNumber.Length - 4) : phoneNumber;
            return firstPart + lastPart;
        }
        public Task<string> GenerateOtpAsync()
        {
            var otp = new Random().Next(100000, 999999).ToString();
            return Task.FromResult(otp);
        }

        public async Task<ServiceResopnses<string>> GenerateAndStoreOtpAsync(string email)
        {
            var res = new ServiceResopnses<string>();
            var user = await _userRepository.FindinguserAsync(email);
            if (user == null)
            {
                res.Success = false;
                res.Message = "User's Email doesn't exist";
                res.Data = null;
                return res;
            }
            var otp = await GenerateOtpAsync();
            user.Otp = otp;
            var updateResult = await _userRepository.UpdatingUserAsync(user);

            if (updateResult != null)
            {
                res.Success = true;
                res.Message = "OTP sent to your email";
                res.Data = otp;
            }
            else
            {
                res.Success = false;
                res.Message = "Failed to send user with OTP";
                res.Data = null;
            }

            return res;
        }

        public async Task<ServiceResopnses<string>> OtpIsVerified(string email, string otp)
        {
            var res = new ServiceResopnses<string>();
            var user = await _userRepository.FindinguserAsync(email);
            if (user == null)
            {
                res.Success = false;
                res.Message = "User not found";
                return res;
            }

            if (user.Otp == otp)
            {
                user.isVerified = true;
                var updateResult = await _userRepository.UpdatingUserAsync(user);

                if (updateResult != null)
                {
                    res.Success = true;
                    res.Message = "Otp Verified";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Failed to update user verification status";
                    return res;
                }
            }
            res.Success = false;
            res.Message = "Wrong Otp";
            return res;
        }

        public async Task<ServiceResopnses<string>> UpdatePasswordAsync(string email, string password)
        {
            var res = new ServiceResopnses<string>();
            var user = await _userRepository.FindinguserAsync(email);
            if (user == null)
            {
                res.Success = false;
                res.Message = "User email doesn't Exists";
                res.Data = null;
                return res;
            }

            if (user.isVerified != true)
            {
                res.Success = false;
                res.Message = "Otp not verified yet";
                res.Data = null;
                return res;
            }
            user.Password = password;
            user.Otp = "******";
            user.isVerified = false;
            var Ok = await _userRepository.UpdatingUserAsync(user);
            if (Ok != null)
            {
                res.Success = true;
                res.Message = "Password updated successfully";
                return res;
            }
            res.Success = false;
            res.Message = "Failed to update the password";
            return res;
        }

        public async Task<ServiceResopnses<string>> AddToCart(AddToCartRequestDTO addToCartDTO)
        {
            var userExists = await _userRepository.FindinguserById(addToCartDTO.UserId);
            var res = new ServiceResopnses<string>();
            if (userExists == null)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "The user Id doesn't Exists";
                return res;
            }

            var product = await _sellerRepositary.GetProductByID(addToCartDTO.ProductId);

            if (product == null)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "The product with this id doesn't exists";
                return res;
            }

            
            if (product.StockQuantitty == 0)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "Sorry there's No Stock";
            }
            if (addToCartDTO.Quantity > product.StockQuantitty)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "The Stock of the quantity is less than you mentioned";
            }
            var totalPrice = addToCartDTO.Quantity * product.ProductPrice;
            var addingToCart = new TblCartItem()
            {
                CartId = addToCartDTO.CartId,  // Set CartId from DTO
                ProductId = addToCartDTO.ProductId,  // Set ProductId from DTO
                Quantity = addToCartDTO.Quantity,  // Set Quantity from DTO
                TotalPrice = totalPrice  // Set TotalPrice from DTO
            };

            var done = _userRepository.AddToCart(addingToCart);
            if (!await done)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "Failed to add Item to the cart";
            }
            res.Success = true;
            res.Message = "Item Succesfully added to the Cart";
            res.Data = null;
            return res;
        }

        public async Task<ServiceResopnses<List<CartDetailsByCartIdResponseDTO>>> GetCartDetailsByCartId(int cartId)
        {
            var res = new ServiceResopnses<List<CartDetailsByCartIdResponseDTO>>();

            // Check if the cart exists
            var cartIdExists = await _userRepository.CheckIfCartExists(cartId);
            if (!cartIdExists)
            {
                res.Message = $"The cart with ID {cartId} doesn't exist.";
                res.Success = false;
                res.Data = null;
                return res;
            }

            try
            {
                // Fetch the JSON result from the stored procedure
                var jsonCartDetails = await _userRepository.GetCartDetailsByCartId(cartId);

                // Ensure it's converted to a string
                var jsonCartString = jsonCartDetails?.ToString(); // Convert to string if necessary

                // If no data is returned or an empty string is provided, handle accordingly
                if (string.IsNullOrEmpty(jsonCartString))
                {
                    res.Success = false;
                    res.Message = "Cart details not found.";
                    res.Data = null;
                    return res;
                }

                // Deserialize the JSON string into the DTO list
                var cartItems = JsonSerializer.Deserialize<List<CartDetailsByCartIdResponseDTO>>(jsonCartString);


                // Check if deserialization returned any items
                if (cartItems == null || !cartItems.Any())
                {
                    res.Success = false;
                    res.Message = "Cart details not found.";
                    res.Data = null;
                    return res;
                }

                // Return success response with the deserialized data
                res.Success = true;
                res.Message = "Cart details retrieved successfully.";
                res.Data = cartItems;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"An error occurred: {ex.Message}";
                res.Data = null;
            }

            return res;
        }

        public async Task<ServiceResopnses<List<ProductsByCategoryIdResponseDTO>>> GetProductsByCategoryID(int categoryId)
        {
            var res = new ServiceResopnses<List<ProductsByCategoryIdResponseDTO>>();

            // Check if the cart exists
            var categoryIdExists = await _userRepository.CheckIfCategoryExistsById(categoryId);
            if (!categoryIdExists)
            {
                res.Message = $"The ctegory with ID {categoryId} doesn't exist.";
                res.Success = false;
                res.Data = null;
                return res;
            }

            try
            {
                // Fetch the JSON result from the stored procedure
                var jsonProducts = await _userRepository.GetProductsByCategoryId(categoryId);

                // Ensure it's converted to a string
                var jsonProductString = jsonProducts?.ToString(); // Convert to string if necessary

                // If no data is returned or an empty string is provided, handle accordingly
                if (string.IsNullOrEmpty(jsonProductString))
                {
                    res.Success = false;
                    res.Message = "Catgeory details not found.";
                    res.Data = null;
                    return res;
                }

                // Deserialize the JSON string into the DTO list
                var products = JsonSerializer.Deserialize<List<ProductsByCategoryIdResponseDTO>>(jsonProductString);


                // Check if deserialization returned any items
                if (products == null || !products.Any())
                {
                    res.Success = false;
                    res.Message = "Products details not found.";
                    res.Data = null;
                    return res;
                }

                // Return success response with the deserialized data
                res.Success = true;
                res.Message = "Products details retrieved successfully.";
                res.Data = products;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"An error occurred: {ex.Message}";
                res.Data = null;
            }

            return res;
        }

        public async Task<ServiceResopnses<List<FlashSalesProductsResponseDTO>>> GetFlashSaleProducts()
        {
            var res = new ServiceResopnses<List<FlashSalesProductsResponseDTO>>();

            try
            {
                // Fetch the flash sales products from the repository
                var products = await _userRepository.GetFlashSalesProducts();

                // Check if data is retrieved
                if (products == null || !products.Any())
                {
                    res.Success = false;
                    res.Message = "No flash sale products found.";
                    res.Data = null;
                    return res;
                }

                // Return success response with the data
                res.Success = true;
                res.Message = "Flash sale products retrieved successfully.";
                res.Data = products;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"An error occurred: {ex.Message}";
                res.Data = null;
            }

            return res;
        }

        
    }
}
