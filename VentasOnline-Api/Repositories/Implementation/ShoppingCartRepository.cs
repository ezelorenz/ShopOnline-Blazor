using Microsoft.EntityFrameworkCore;
using VentasOnline.Models.Dto;
using VentasOnline_Api.Data;
using VentasOnline_Api.Entities;
using VentasOnline_Api.Repositories.Contracts;

namespace VentasOnline_Api.Repositories.Implementation
{
	public class ShoppingCartRepository : IShoppingCartRepository
	{
		private readonly VentasOnlineDbContext db;
		public ShoppingCartRepository(VentasOnlineDbContext dataBase)
		{
			db = dataBase;
		}
		private async Task<bool> CartItemExist(int cartId, int productId)
		{
			return await db.CartItems.AnyAsync(c=> c.CartId == cartId && 
												c.ProductId == productId);
		}
        public async Task<bool>DeleteCartItem(int cartId)
		{

			db.CartItems.RemoveRange(db.CartItems.Where(x=> x.CartId == cartId));

            await db.SaveChangesAsync();
            return true;

        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
		{
			if(await CartItemExist(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
			{
                var item = await (from p in db.Products
                                  where p.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = p.Id,
                                      Quantity = cartItemToAddDto.Quantity
                                  }).SingleOrDefaultAsync();
                if (item != null)
                {
                    var result = await db.CartItems.AddAsync(item);
                    await db.SaveChangesAsync();
                    return result.Entity;
                }
            }
			
			return null;
        }

        public async Task<CartItem> GetItem(int id)
        {
			return await (from c in db.Carts
						  join ci in db.CartItems
						  on c.Id equals ci.CartId
						  where ci.Id == id
						  select new CartItem
						  {
							  Id = ci.Id,
							  ProductId = ci.ProductId,
							  CartId = ci.CartId,
							  Quantity = ci.Quantity
						  }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
			return await (from c in db.Carts
						  join ci in db.CartItems
						  on c.Id equals ci.CartId
						  where c.UserId == userId
						  select new CartItem
						  {
							  Id = ci.Id,
							  CartId = ci.CartId,
							  ProductId=ci.ProductId,
							  Quantity = ci.Quantity
						  }).ToListAsync();
        }

        public async Task<CartItem> DeleteItem(int id)
		{
			var item = await db.CartItems.FindAsync(id);

			if(item != null)
			{
				db.CartItems.Remove(item);
				await db.SaveChangesAsync();
			}
			return item;
		}


		public async Task<CartItem> UpdateQuantity(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
		{
			var item = await db.CartItems.FindAsync(id);
			if (item != null)
			{
				item.Quantity = cartItemQtyUpdateDto.Quantity;
				await db.SaveChangesAsync();
				return item;
			}
			return null;
		}
	}
}
