using Microsoft.AspNetCore.Components;
using VentasOnline.Models.Dto;

namespace VentasOnlineWeb.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto>Products { get; set; }
    }
}
