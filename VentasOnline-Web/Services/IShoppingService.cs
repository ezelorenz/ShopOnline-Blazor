using VentasOnline.Models.Dto;

namespace VentasOnlineWeb.Services
{
    public interface IShoppingService
    {
        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> DeleteCartItem(int cartId);
        Task<CartItemDto> UpdateQuantity(CartItemQtyUpdateDto cartItemQtyUpdateDto);
        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQuantity);
    }
}
