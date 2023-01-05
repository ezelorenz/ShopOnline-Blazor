using Microsoft.AspNetCore.Components;
using VentasOnline.Models.Dto;
using VentasOnlineWeb.Services;

namespace VentasOnlineWeb.Pages
{
    public class CheckoutBase : ComponentBase
    {
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        protected decimal PaymentAmount { get; set; }

        [Inject]
        public IShoppingService ShoppingCartService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

            if (ShoppingCartItems != null && ShoppingCartItems.Count() > 0)
            {
                PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
            }
        }


    }
}