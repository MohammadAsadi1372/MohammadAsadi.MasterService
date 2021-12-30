namespace MasterService.EndPoint.Api.Models
{
    public class M_Services
    {
        public string ServiceName { get; set; }
        public string SpName { get; set; }
        public bool HaveLogin { get; set; }
        public List<M_AuthenticateRequest> Login { get; set; }
        public List<M_SpInputs> Inputs { get; set; }
    }
}
