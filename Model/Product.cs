namespace Woolies.Model
{
    public class Product
    {
        public Product(string name, decimal price, double quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public string Name { get; }
        public decimal Price { get; }
        public double Quantity { get; }
    }
}