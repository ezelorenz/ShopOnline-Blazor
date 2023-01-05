using Microsoft.AspNetCore.Components;
using VentasOnline.Models.Dto;
using VentasOnlineWeb.Services;

namespace VentasOnlineWeb.Pages
{
    public class CheckoutBase : ComponentBase
    {
        protected List<CartItemDto> ShoppingCartItems { get; set; }
        protected decimal PaymentAmount { get; set; }
        public string Message { get; set; }
        protected int TotalQuantity { get; set; } = 0;

        [Inject]
        public IShoppingService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

            if (ShoppingCartItems != null && ShoppingCartItems.Count() > 0)
            {
                PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
            }
        }
        protected void Purchase()
        {
            
            RemoveCartItem(HardCoded.CartId);
            NavigationManager.NavigateTo("/");
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }

        private async Task RemoveCartItem(int cartId)
        {
            await ShoppingCartService.DeleteCartItem(cartId);
        }

        
    }
}