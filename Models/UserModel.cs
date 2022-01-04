using System.ComponentModel.DataAnnotations;

namespace LunchGuide.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Restaurant { get; set; }
    }
}
