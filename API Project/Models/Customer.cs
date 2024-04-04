using System.ComponentModel.DataAnnotations;

namespace API_Project.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

    }
}
