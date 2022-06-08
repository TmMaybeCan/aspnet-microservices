using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Interfaces
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var result = await _catalogContext.Products.Find(p => true).ToListAsync();

            return result;
        }

        public async Task<Product> GetById(string id)
        {
            var result = await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Product>> GetByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            var result = await _catalogContext.Products.Find(filter).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Product>> GetByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            var result = await _catalogContext.Products.Find(filter).ToListAsync();

            return result;
        }

        public async Task Create(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);

        }

        public async Task<bool> Update(Product product)
        {
            //if (id != product.Id)
            //    return false;

            var result = await _catalogContext.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                 
            var result = await _catalogContext.Products.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

    }
}
