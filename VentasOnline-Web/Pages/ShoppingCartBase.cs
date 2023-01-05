using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VentasOnline.Models.Dto;
using VentasOnlineWeb.Services;

namespace VentasOnlineWeb.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingService ShoppingService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        protected string TotalPrice { get;set; }
        protected int TotalQuantity { get;set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingService.GetItems(HardCoded.UserId);
                CartChanged();
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingService.DeleteItem(id);
            RemoveCartItem(id);
            CartChanged();

        }
        protected CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }
        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
        }

        protected async Task UpdateQuantityCartItem_Click(int id, int quantity)
        {
            try
            {
                if(quantity > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Quantity = quantity
                    };

                    var returnedUpdateItemDto = await ShoppingService.UpdateQuantity(updateItemDto);
                    await UpdateItemTotalPrice(returnedUpdateItemDto);
                    CartChanged();
                    await VisibleButtonUpdateQty(id, false);
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);
                    if(item != null)
                    {
                        item.Quantity = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected async Task UpdateQty_Input(int id)
        {
            await VisibleButtonUpdateQty(id, true);
        }

        private async Task VisibleButtonUpdateQty(int id, bool visible)
        {
            await Js.InvokeVoidAsync("VisibleButtonUpdateQty", id, visible);
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString();
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(q => q.Quantity);
        }
        private void CalculateCartSummaryTotal()
        {
            SetTotalPrice();
            SetTotalQuantity();

        }
        private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if(item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Quantity;
            }

        }

        private void CartChanged()
        {
            CalculateCartSummaryTotal();
            ShoppingService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}
