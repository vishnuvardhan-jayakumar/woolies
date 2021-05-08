using System.Collections.Generic;
using System.Linq;
using Woolies.Model;

namespace Woolies.Services
{
    public class BestPriceCalculationService
    {
        public decimal CalculateBestPrice(Product[] products, Special[] specials, ShoppingCartItem[] shoppingCartItems)
        {
            var productToPriceLookup = products.ToDictionary(p => p.Name, p => p.Price);

            var productsInCart = shoppingCartItems.Select(t => t.Name).ToHashSet();
            
            // filter specials which are only applicable to the current item
            var applicableSpecials =
                specials.Where( s=> CanApplySpecial(shoppingCartItems, s));
            
            // very basic algorithm
            // calculate all permutation of special
            // apply them and check the lowest price
            // prevent recalculation 
            return 0;
        }

        
        /*private static IEnumerable<ShoppingCartItem> ApplySpecial(Special special,
            List<ShoppingCartItem> shoppingCartItems)
        {
            shoppingCartItems.
        }*/
        
        
        // https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private static bool CanApplySpecial(ShoppingCartItem[] shoppingCartItems, Special special)
        {
            return special.Quantities.All(specialItem =>
            {
                return shoppingCartItems.Any(sci =>
                    sci.Name == specialItem.Name && sci.Quantity >= specialItem.Quantity);
            });
        }
    }

    public interface IBestPriceCalculationService
    {
        public decimal CalculateBestPrice(Product[] products, Special[] specials, ShoppingCartItem[] shoppingCartItems);
    }
}