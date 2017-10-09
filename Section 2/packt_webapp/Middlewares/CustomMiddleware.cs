using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;

namespace packt_webapp.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private MyConfiguration _myconfig;

        public CustomMiddleware(RequestDelegate next, IOptions<MyConfiguration> myconfig)
        {
            _next = next;
            _myconfig = myconfig.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Debug.WriteLine($" ---> Request asked for {httpContext.Request.Path} from {_myconfig.Firstname} {_myconfig.Lastname}");

            // Call the next middleware delegate in the pipeline 
            await _next.Invoke(httpContext);
        }
    }
}
