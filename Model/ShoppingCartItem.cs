namespace Woolies.Model
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; }
        public int Quantity { get; }
    }
}