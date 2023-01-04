﻿using Microsoft.EntityFrameworkCore;
using VentasOnline_Api.Data;
using VentasOnline_Api.Entities;
using VentasOnline_Api.Repositories.Contracts;

namespace VentasOnline_Api.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly VentasOnlineDbContext db;
        public ProductRepository(VentasOnlineDbContext dataBase)
        {
            db = dataBase;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            return await db.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            return await db.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> GetItem(int id)
        {
            return await db.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            return await db.Products.ToListAsync();
        }
    }
}
