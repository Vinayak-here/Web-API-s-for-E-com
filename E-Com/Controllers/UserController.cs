using E_Com.DTO_s.RequestDTO_s;
using E_Com.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace E_Com.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userServices;
        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegistrationRequestDTO regDto)
        {
            var reg = await _userServices.RegistrationAsync(regDto);
            if (reg.Success)
            {
                return Ok(reg);
            }
            else
            {
                return BadRequest(reg);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginDto)
        {
            var result = await _userServices.Loginasync(loginDto);
            if (result.Success)
            {
                return Ok(new { message = result.Message, username = result.Data });
            }
            return BadRequest(result);
        }



        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userServices.GenerateAndStoreOtpAsync(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPut("OtpVerification")]
        public async Task<IActionResult> OtpConfirmation(string email, string otp)
        {
            var result = await _userServices.OtpIsVerified(email, otp);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("UpdatPassword")]
        public async Task<IActionResult> UpdatingPassword(string email, string password)
        {
            var result = await _userServices.UpdatePasswordAsync(email, password);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("AddToCart")]

        public async Task<IActionResult> AddToCart(AddToCartRequestDTO addToCartRequestDTO)
        {
            var result = await _userServices.AddToCart(addToCartRequestDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetCartDetailsByCartId")]
        public async Task<IActionResult> GetCartDetailsByCartId(int id)
        {
            var result = await _userServices.GetCartDetailsByCartId(id);
            if (result.Success)
            { return Ok(result); }
            return BadRequest(result);
        }
        [HttpGet("GetProductsByCategoryId")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var result = await _userServices.GetProductsByCategoryID(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpGet("GetFlashSaleProducts")]
        public async Task<IActionResult> GetFlashSaleProducts()
        {
            var result = await _userServices.GetFlashSaleProducts();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


    }
    
}
