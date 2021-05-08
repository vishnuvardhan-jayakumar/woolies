namespace Woolies.Model
{
    public class TrolleyCalculatorRequest
    {
        public TrolleyCalculatorRequest(Product[] products, Special[] specials, ShoppingCartItem[] quantities)
        {
            Products = products;
            Specials = specials;
            Quantities = quantities;
        }

        public Product[] Products { get; }
        public Special[] Specials { get; }
        public ShoppingCartItem[] Quantities { get; }
    }
}