using Catalog.API.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(string id);
        Task<IEnumerable<Product>> GetByName(string name);
        Task<IEnumerable<Product>> GetByCategory(string category);
        Task Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(string id);


    }
}
