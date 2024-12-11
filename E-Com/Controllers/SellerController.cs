using E_Com.DTO_s.RequestDTO_s;
using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Com.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpPost("SellerRegister")]
        public async Task<IActionResult> SellerRegister([FromBody] SellerRegistrationRequestDTO sellerRegistrationDTO)
        {
            var done = await _sellerService.SellerRegister(sellerRegistrationDTO);
            if (done != null)
            {
                return Ok(done);
            }
            return BadRequest(done);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductRequestDTO addProductRequestDTO)
        {
            var done = await _sellerService.AddProduct(addProductRequestDTO);
            if (done != null)
            {
                return Ok(done);
            }
            return BadRequest(done);

        }

        [HttpGet("GetProductsByCategoryId")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var products = await _sellerService.GetProductByCategory(categoryId);
            if (products != null)
            {
                return Ok(products);
            }
            return BadRequest(products);
        }



        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var result = await _sellerService.GetProductById(productId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetSellerDetailsByProductId")]
        public async Task<IActionResult> GetSellerDetailsByProductId(int id)
        {
            var result = await _sellerService.GetSellerByProductId(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetProductAndSellerDetailsByProdId")]
        public async Task<IActionResult> GetProductAndSellerDetailsByProdId(int prodId)
        {
            var result = await _sellerService.GetSellerByProductId(prodId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _sellerService.DeleteProductById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
