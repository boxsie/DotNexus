using System.ComponentModel.DataAnnotations;

namespace DotNexus.App.Models
{
    public class ConnectionConnectModel
    {
        [Required]
        public string NodeId { get; set; }
    }
}