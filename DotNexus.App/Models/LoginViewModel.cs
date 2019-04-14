using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNexus.App.Models
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("PIN")]
        public int? Pin { get; set; }
    }
}