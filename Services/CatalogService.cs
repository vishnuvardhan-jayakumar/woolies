using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Woolies.Model;

namespace Woolies.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IResourceService _resourceService;

        public CatalogService(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public async Task<IEnumerable<Product>> GetSortedProducts(SortBy sortBy)
        {
            var products = await _resourceService.GetProducts();
            return sortBy switch
            {
                SortBy.Low => products.OrderBy(product => product.Price),
                SortBy.High => products.OrderByDescending(product => product.Price),
                SortBy.Ascending => products.OrderBy(product => product.Name),
                SortBy.Descending => products.OrderByDescending(product => product.Name),
                SortBy.Recommended => await GetProductsByPopularity(products),
                _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null)
            };
        }

        private async Task<IEnumerable<Product>> GetProductsByPopularity(IEnumerable<Product> products)
        {
            var productsArray = products.ToArray();
            var shoppingHistory = (await _resourceService.GetShoppingHistory()).ToArray();

            var popularityByNumberOfOrders = 
                productsArray.ToDictionary(product => product.Name,
                    product => shoppingHistory.Count(order => order.Products.Select(p=> p.Name).Contains(product.Name)));
            
            var grouping = shoppingHistory.SelectMany(y => y.Products)
                .GroupBy(r => r.Name);
          var popularityByTotalCount =  grouping.ToDictionary(g => g.Key, g=> g.Count());
          
          // i couldn't find the definition of popularity. I am assuming this could be it
          return productsArray.OrderByDescending(product =>
                  popularityByNumberOfOrders.ContainsKey(product.Name) ? popularityByNumberOfOrders[product.Name] : 0)
              .ThenByDescending(product =>
                  popularityByTotalCount.ContainsKey(product.Name) ? popularityByTotalCount[product.Name] : 0);
          //  .ThenByDescending(product => product.Price);
        }
    }
}