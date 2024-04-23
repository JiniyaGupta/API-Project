using System.ComponentModel.DataAnnotations;

namespace API_Project.Models
{
    public class Authentication
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
