using ETender.Products.Data.Interfaces;
using ETender.Products.Entities;
using ETender.Products.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ETender.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;
        public ProductRepository(IProductContext context)
        {
            _context = context;
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var result = await _context.Products.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string name)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Category, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.ElemMatch(x => x.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(x => true).ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var result = await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
