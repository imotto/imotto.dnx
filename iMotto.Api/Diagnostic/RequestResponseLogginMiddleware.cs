using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Api.Diagnostic
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                .CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var initialBody = context.Request.Body;
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                context.Request.EnableRewind();
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, 4096, true))
                {
                    var reqBody = await reader.ReadToEndAsync();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);

                    _logger.LogWarning("[**REQUEST**]:\n\t{0}", reqBody);
                }

                await _next(context);

                context.Request.Body = initialBody;
                var resp = await FormatResponse(context.Response);
                _logger.LogWarning("[**RESPONSE**]:\n\t{0}", resp);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }

    }
}
