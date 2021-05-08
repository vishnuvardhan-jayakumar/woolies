using System.Collections.Generic;
using System.Threading.Tasks;
using Woolies.Model;

namespace Woolies.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<Product>> GetSortedProducts(SortBy sortBy);
    }
}