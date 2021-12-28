using MasterService.EndPoint.Api.Helper;

namespace MasterService.EndPoint.Api.Middleware
{
    public class AuthenticationMiddleWare
    {
        private RequestDelegate nextDelegate;
        private readonly IConfiguration config;

        public AuthenticationMiddleWare(RequestDelegate next,IConfiguration config)
        {
            nextDelegate = next;
            this.config = config;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var Path = httpContext.Request.Path;
            if (!Path.Value.Contains("Login"))
            {
                if (IsUserValid(httpContext))
                    await nextDelegate.Invoke(httpContext);
                else
                    await httpContext.Response
                        .WriteAsync($"Status Code: {401} " + Environment.NewLine + "Status Message:  Security error ");
            }
            else
                await nextDelegate.Invoke(httpContext);
        }

        private bool IsUserValid(HttpContext httpContext)
        {
            if (new H_JWT(this.config).ISValidToken(httpContext.Request.Headers["X-ValidToken"])
                && !string.IsNullOrEmpty(httpContext.Request.Headers["X-ValidToken"]))
                return true;
            else
                return false;
        }

    }
}
