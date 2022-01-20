using System.Text;

namespace ProductsArchive.WebUI.Middlewares;

public class HttpRequestLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public HttpRequestLoggerMiddleware(RequestDelegate requestDelegate, ILogger<HttpRequestLoggerMiddleware> logger)
    {
        _next = requestDelegate;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        string baseUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString.Value}";

        StringBuilder sbHeaders = new StringBuilder();
        foreach (var header in request.Headers)
        {
            sbHeaders.Append($"\n{header.Key}: {header.Value}");
        }

        string body = "no-body";

        if (request.Body.CanSeek)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(request.Body))
                body = sr.ReadToEnd();
        }

        //$"{request.Protocol} {request.Method} {baseUrl}\n\n{sbHeaders}\n{body}"
        _logger.LogInformation($"\nProtocol: {request.Protocol}\n" +
            $"Method: {request.Method}\n" +
            $"Url: {baseUrl}" +
            $"{sbHeaders}\n" +
            $"{body}");

        await _next(context);
    }
}

public static class HttpRequestLoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseHttpRequestLogger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpRequestLoggerMiddleware>();
    }
}
