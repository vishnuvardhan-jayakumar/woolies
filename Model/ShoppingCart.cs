namespace Woolies.Model
{
    public class ShoppingCart
    {
        public ShoppingCart(Product[] product, Special[] specials, ShoppingCartItem quantities)
        {
            Product = product;
            Specials = specials;
            Quantities = quantities;
        }

        public Product[] Product { get; }
        public Special[] Specials { get; }
        public ShoppingCartItem Quantities { get; } 
    }
}