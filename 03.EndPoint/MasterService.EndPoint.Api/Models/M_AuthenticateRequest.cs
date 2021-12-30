using System.ComponentModel.DataAnnotations;

namespace MasterService.EndPoint.Api.Models
{
    public class M_AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
