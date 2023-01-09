using VentasOnline.Models.Dto;

namespace VentasOnlineWeb.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductDto> GetItem(int id);
        Task<IEnumerable<ProductCategoryDto>> GetProductCategories();
    }
}
