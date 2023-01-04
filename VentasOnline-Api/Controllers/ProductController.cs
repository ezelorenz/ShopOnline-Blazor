using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VentasOnline.Models.Dto;
using VentasOnline_Api.Extensions;
using VentasOnline_Api.Repositories.Contracts;

namespace VentasOnline_Api.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await _repo.GetItems();
                var productsCategories = await _repo.GetCategories();

                if (products == null || productsCategories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productsCategories);
                    return Ok(productDtos);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await _repo.GetItem(id);
                

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productCategory = await _repo.GetCategory(product.CategoryId);

                    var productDto = product.ConvertToDto(productCategory);
                    return Ok(productDto);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
