using System.ComponentModel.DataAnnotations;

namespace MasterService.EndPoint.Api.Models
{
    public class M_AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
