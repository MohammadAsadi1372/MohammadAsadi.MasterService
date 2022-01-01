using MasterService.EndPoint.Api.Helper;
using MasterService.EndPoint.Api.Models;
using System.Net;

namespace MasterService.EndPoint.Api.Middleware
{
    public class AuthenticationMiddleWare
    {
        private RequestDelegate nextDelegate;
        private readonly IConfiguration _config;

        public AuthenticationMiddleWare(RequestDelegate next, IConfiguration config)
        {
            nextDelegate = next;
            this._config = config;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var Data = _config.GetSection("Services").Get<List<M_Services>>();
                if (Data != null)
                {
                    string SystemName = httpContext.Request.Headers["X-SystemName"];
                    if (string.IsNullOrEmpty(SystemName))
                    {
                        httpContext.Response.Clear();
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await httpContext.Response
                            .WriteAsync($"Status Code: {StatusCodes.Status400BadRequest} " + Environment.NewLine + $"Status Message: نام سیستم را مشخص کنید");
                    }
                    else
                        foreach (var item in Data)
                            if (item.ServiceName == SystemName)
                                if (!item.HaveLogin)
                                    await nextDelegate.Invoke(httpContext);
                                else
                                {
                                    var Path = httpContext.Request.Path;
                                    if (!Path.Value.Contains("Login"))
                                    {
                                        if (IsUserValid(httpContext, SystemName))
                                            await nextDelegate.Invoke(httpContext);
                                        else
                                        {
                                            httpContext.Response.Clear();
                                            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                            await httpContext.Response
                                                .WriteAsync($"Status Code: {401} " + Environment.NewLine + "Status Message:  Security error ");
                                        }
                                    }
                                    else
                                        await nextDelegate.Invoke(httpContext);
                                }
                }
                else
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await httpContext.Response
                        .WriteAsync($"Status Code: {StatusCodes.Status400BadRequest} " + Environment.NewLine + $"Status Message: تنظیمات سیستم را وارد نمایید");
                }
            }
            catch (Exception ex)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response
                    .WriteAsync($"Status Code: {StatusCodes.Status400BadRequest} " + Environment.NewLine + $"Status Message:  {ex.Message} ");
            }
        }

        private bool IsUserValid(HttpContext httpContext,string SystemName)
        {
            if (new H_JWT(this._config, SystemName).ISValidToken(httpContext.Request.Headers["X-ValidToken"])
                && !string.IsNullOrEmpty(httpContext.Request.Headers["X-ValidToken"]))
                return true;
            else
                return false;
        }

    }
}
