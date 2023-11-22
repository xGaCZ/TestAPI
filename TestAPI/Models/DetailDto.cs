using System.ComponentModel.DataAnnotations;

namespace TestNetAPI.Models
{
    public class DetailDto
    { 
      
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Password { get; set; }
        public DateTime Born { get; set; }
    }
}
