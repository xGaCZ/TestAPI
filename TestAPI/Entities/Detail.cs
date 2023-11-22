using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestNetAPI.Entities
{
    public class Detail
    {
        public int Id { get; set; }
        public string LastName { get; set; } 
        public string Email { get; set; } 
        public string Category { get; set; } 
        public string Subcategory { get; set; } 
        public string Password { get; set; } 
        public DateTime Born { get; set; }  
        public int ContactID { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
