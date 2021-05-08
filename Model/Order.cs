namespace Woolies.Model
{
    public class Order
    {
        public Order(int customerId, Product[] products)
        {
            CustomerId = customerId;
            Products = products;
        }

        public int CustomerId { get; }
        public Product[] Products { get; }
    }
}
