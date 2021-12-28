namespace MasterService.EndPoint.Api.Models
{
    public class M_MasterResponse
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public dynamic Result { get; set; }

        public M_MasterResponse()
        {

        }

        public M_MasterResponse UnAouthorize()
        {
            return new M_MasterResponse
            {
                Status = false,
                ErrorMessage = "خطای اعتبارسنجی",
                Result = null,
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        public M_MasterResponse BadRequest(string message)
        {
            return new M_MasterResponse
            {
                Status = false,
                ErrorMessage = message,
                Result = null,
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

    }
}
