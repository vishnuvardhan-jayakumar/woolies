namespace Woolies.Model
{
    public class Special
    {
        public Special(ShoppingCartItem[] quantities, decimal total)
        {
            Quantities = quantities;
            Total = total;
        }

        public ShoppingCartItem[] Quantities { get; }
        public decimal Total { get; }
    }
}