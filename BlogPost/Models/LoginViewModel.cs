using System.ComponentModel.DataAnnotations;

namespace BlogPost.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Name { get; set; } = "";

        [Required] 
        [EmailAddress] 
        public string Email { get; set; } = "";

        [Required]
        public int Age { get; set; }
    }
}