using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNexus.App.Models
{
    public class CreateConnectionViewModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("URL")]
        public string Url { get; set; }

        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
        
        [DisplayName("Session enabled")]
        public bool ApiSessions { get; set; }

        [DisplayName("Height index enabled")]
        public bool IndexHeight { get; set; }
    }
}