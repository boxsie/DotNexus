using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DotNexus.Core.Enums;

namespace DotNexus.App.Models
{
    public class CreateTokenViewModel
    {
        [Required]
        [DisplayName("Type")]
        public TokenType TokenType { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("ID")]
        public string Identifier { get; set; }

        [DisplayName("PIN")]
        public int? Pin { get; set; }

        [DisplayName("Total supply")]
        public double? TotalSupply { get; set; }
    }
}