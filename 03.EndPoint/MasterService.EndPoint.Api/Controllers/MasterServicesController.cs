using MasterService.Core.ApplicationService.Query;
using MasterService.EndPoint.Api.Helper;
using MasterService.EndPoint.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MasterService.EndPoint.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterServicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public MasterServicesController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody] M_AuthenticateRequest date)
        {
            try
            {
                int Hours = Convert.ToInt32(_config.GetSection("ExpireTokenByHour").Value);
                var Result = new H_JWT(date.Username, date.Password, DateTime.Now.AddHours(Hours), _config).Generator();
                return Ok(new
                {
                    result = Result.Item1,
                    status = Result.Item2
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    result = ex.Message,
                    status = false
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetData(
            [FromHeader(Name = "X-ValidToken")][Required] string MasterServiceToken,
            [FromBody] FetchQuery requset)
        {
            try
            {
                var ress = await _mediator.Send(requset);
                return Ok("Sucess Token");
            }
            catch (Exception ex)
            {
                return BadRequest(new M_MasterResponse().BadRequest(ex.Message));
            }
        }

    }

}
