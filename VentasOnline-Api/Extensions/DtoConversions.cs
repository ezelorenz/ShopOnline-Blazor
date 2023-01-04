using System.Runtime.CompilerServices;
using VentasOnline.Models.Dto;
using VentasOnline_Api.Entities;

namespace VentasOnline_Api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
                                                           IEnumerable<ProductCategory> productCategories)
        {
            return (from p in products
                    join pc in productCategories
                    on p.CategoryId equals pc.Id
                    select new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImageURL = p.ImageURL,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        CategoryId = p.CategoryId,
                        CategoryName = pc.Name
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product p, ProductCategory pc)
        {
            return  new ProductDto
                    {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageURL = p.ImageURL,
                Price = p.Price,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId,
                CategoryName = pc.Name
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItem, 
                                                                IEnumerable<Product> product)
        {
            return (from ci in cartItem join p in product
                    on ci.ProductId equals p.Id
                    select new CartItemDto
                    {
                        Id = ci.Id,
                        ProductId = p.Id,
                        CartId = ci.CartId,
                        ProductName = p.Name,
                        ProductDescription = p.Description,
                        ProductImageURL = p.ImageURL,
                        Price = p.Price,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Quantity * p.Price
                    }).ToList();
        }
        public static CartItemDto ConvertToDto(this CartItem ci, Product p)
        {
            return new CartItemDto
            {
                Id = ci.Id,
                ProductId = p.Id,
                CartId = ci.CartId,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductImageURL = p.ImageURL,
                Price = p.Price,
                Quantity = ci.Quantity,
                TotalPrice = ci.Quantity * p.Price
            };
        }
    }
}
