using Microsoft.AspNetCore.Components;
using VentasOnline.Models.Dto;
using VentasOnlineWeb.Services;

namespace VentasOnlineWeb.Shared
{
    public class ProductCategoryNavMennuBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductCategoryDto> productCategoryDtos { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                productCategoryDtos = await ProductService.GetProductCategories();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
