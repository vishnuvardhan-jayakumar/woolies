using System.Collections.Generic;
using System.Threading.Tasks;
using Woolies.Model;

namespace Woolies.Services
{
    public interface IResourceService
    {
        public Task<IEnumerable<Order>> GetShoppingHistory();

        public Task<IEnumerable<Product>> GetProducts();

        public Task<decimal> GetTrolleyTotal(TrolleyCalculatorRequest request);
    }
}