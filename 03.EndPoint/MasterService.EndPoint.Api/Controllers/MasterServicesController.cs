using MasterService.Core.ApplicationService.Query;
using MasterService.EndPoint.Api.Helper;
using MasterService.EndPoint.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
        public IActionResult Login(
            [FromHeader(Name = "X-SystemName")] string SystemName,
            [FromBody] M_AuthenticateRequest date)
        {
            try
            {
                int Hours = Convert.ToInt32(_config.GetSection("ExpireTokenByHour").Value);
                var Result = new H_JWT(date.Username, date.Password, DateTime.Now.AddHours(Hours), SystemName, _config).Generator();
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
            [FromHeader(Name = "X-ValidToken")] string MasterServiceToken,
            [FromHeader(Name = "X-SystemName")] string SystemName,
            [FromBody] object requset)
        {
            try
            {
                var Result = await _mediator.Send(new FetchQuery { Request = ConvertObject(requset), SystemName = SystemName });
                return Ok(new M_MasterResponse().Success(Result));
            }
            catch (Exception ex)
            {
                return BadRequest(new M_MasterResponse().BadRequest(ex.Message));
            }
        }

        protected Dictionary<string, object> ConvertObject(object requset)
        {
            string serializedObject = JsonSerializer.Serialize(requset);

            Dictionary<string, object> data = new Dictionary<string, object>();
            var d = JsonDocument.Parse(serializedObject);
            var result = d.RootElement.EnumerateObject();
            foreach (var r in result)
            {
                try
                {
                    if (r.Value.ValueKind == JsonValueKind.String)
                    {
                        var Value = r.Value.GetString();
                        data.Add(r.Name, Value);
                    }
                    else if (r.Value.ValueKind == JsonValueKind.Number)
                    {
                        var Value = r.Value.GetDecimal();
                        data.Add(r.Name, Value);
                    }
                    else if (r.Value.ValueKind == JsonValueKind.True)
                    {
                        var Value = r.Value.GetBoolean();
                        data.Add(r.Name, Value);
                    }
                    else if (r.Value.ValueKind == JsonValueKind.Null)
                    {
                        data.Add(r.Name, null);
                    }
                }
                catch
                {
                    continue;
                }
            }
            return data;
        }
    }
}
