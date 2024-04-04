namespace API_Project.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid ID { get; set; }

        public Guid CustomerId { get; set; }

        //public string CustomerName { get; set; }
        public int quantity { get; set; }


    }
}
