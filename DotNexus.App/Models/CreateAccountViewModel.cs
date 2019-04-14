using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNexus.App.Models
{
    public class CreateAccountViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("PIN")]
        public int? Pin { get; set; }

        [DisplayName("Login immediately")]
        public bool AutoLogin { get; set; }
    }
}