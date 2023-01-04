using VentasOnline.Models.Dto;
using VentasOnline_Api.Entities;

namespace VentasOnline_Api.Repositories.Contracts
{
	public interface IShoppingCartRepository
	{
		Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto);
		Task<CartItem>UpdateQuantity (int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
		Task<CartItem>DeleteItem(int id);
		Task<CartItem> GetItem (int id);
		Task<IEnumerable<CartItem>> GetItems(int userId);
	}
}
