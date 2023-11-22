using System.ComponentModel.DataAnnotations;

namespace TestNetAPI.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Phone]
        [Required]
        public string Phone { get; set; } 
        public virtual Detail Detail { get; set; }

    }
}
