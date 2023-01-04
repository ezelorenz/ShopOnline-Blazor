using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using VentasOnline.Models.Dto;
using VentasOnlineWeb.Services;

namespace VentasOnlineWeb.Pages
{
    public class ProductsDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingService ShoppingService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public ProductDto Product { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await ProductService.GetItem(Id);
                var shoppingCartItems = await ShoppingService.GetItems(HardCoded.UserId);
                var totalQty = shoppingCartItems.Sum(i => i.Quantity);

                ShoppingService.RaiseEventOnShoppingCartChanged(totalQty);
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await ShoppingService.AddItem(cartItemToAddDto);
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
