using System.ComponentModel.DataAnnotations;

namespace TestNetAPI.Models
{
    public class RegisterUserDto
    {

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } =2;
    }
}
