using System.ComponentModel.DataAnnotations;

namespace API_Project.Models
{
    public class Brand
    {
        [Key]
        public Guid ID { get; set; }

        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

    }
}
