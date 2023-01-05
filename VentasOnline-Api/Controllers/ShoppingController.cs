using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VentasOnline.Models.Dto;
using VentasOnline_Api.Extensions;
using VentasOnline_Api.Repositories.Contracts;

namespace VentasOnline_Api.Controllers
{
	[EnableCors("ReglasCors")]
    [Route("api/[controller]")]
	[ApiController]
	public class ShoppingController : ControllerBase
	{
		private readonly IShoppingCartRepository _repoCart;
		private readonly IProductRepository _repoProd;
		public ShoppingController(IShoppingCartRepository repo, IProductRepository repoProd)
		{
			_repoCart = repo;
			_repoProd = repoProd;
		}

		[HttpGet]
		[Route("{userId}/GetItems")]
		public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
		{
			try
			{
				var cartItems = await _repoCart.GetItems(userId);
				if(cartItems == null)
				{
					return NoContent();
				}

				var products = await _repoProd.GetItems();
				if(products == null)
				{
					throw new Exception("No products exist in the system");
				}
				var cartItemsDto = cartItems.ConvertToDto(products);

				return Ok(cartItemsDto);

			}
			catch(Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CartItemDto>>GetItem(int id)
		{
			try
			{
				var cartItem = await _repoCart.GetItem(id);
				if( cartItem == null)
				{
					return NotFound();
				}

				var product = await _repoProd.GetItem(cartItem.ProductId);
                if (product == null)
                {
                    return NotFound();
                }
				var cartItemDto = cartItem.ConvertToDto(product);

				return Ok(cartItemDto);
            }
			catch(Exception ex)
			{
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
		}


        [HttpPost]
		public async Task<ActionResult<CartItemDto>> PostItem([FromBody]CartItemToAddDto cartItemToAddDto)
		{
			try
			{
				var newCartItem = await _repoCart.AddItem(cartItemToAddDto);
				if(newCartItem == null)
				{
					return NoContent();
				}
				var product = await _repoProd.GetItem(newCartItem.ProductId);
				if(product == null)
				{
					throw new Exception($"Something went wrong when attemting to retrive product " +
                        $"					(productId:({cartItemToAddDto.ProductId}))");
				}

				var newCartItemDto = newCartItem.ConvertToDto(product);

				return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);
			}
			catch(Exception ex)
			{
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
		}

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var cartItem = await _repoCart.DeleteItem(id);

                if (cartItem == null)
                {
                    return NotFound();
                }

                var product = await _repoProd.GetItem(cartItem.ProductId);

                if (product == null)
                    return NotFound();

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteCartItem/{cartId:int}")]
        public async Task<ActionResult> DeleteCartItem(int cartId)
		{
			try
			{
                var cartItem = await _repoCart.DeleteCartItem(cartId);
                if (cartItem == false)
                {
                    return NotFound();
                }
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQuantity(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await this._repoCart.UpdateQuantity(id, cartItemQtyUpdateDto);
                if (cartItem == null)
                {
					return NotFound();
				}

                var product = await _repoProd.GetItem(cartItem.ProductId);

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
