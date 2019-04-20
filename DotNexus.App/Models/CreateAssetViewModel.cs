using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNexus.App.Models
{
    public class CreateAssetViewModel
    {
        [Required]
        [DisplayName("Asset name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Data")]
        public string Data { get; set; }

        [DisplayName("PIN")]
        public int? Pin { get; set; }
    }
}