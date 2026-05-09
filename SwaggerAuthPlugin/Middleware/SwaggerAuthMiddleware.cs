using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace SwaggerAuthPlugin
{
    public class SwaggerAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SwaggerAuthPluginOptions _options;

        public SwaggerAuthMiddleware(RequestDelegate next, IOptions<SwaggerAuthPluginOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 如果未启用或路径非Swagger，则直接放行
            if (!_options.Enabled || !context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            // 检查用户是否已通过认证
            if (IsAuthenticated(context))
            {
                await _next(context);
                return;
            }

            // 如果未认证且请求的是登录页、登录接口、状态检查接口，则放行
            if (context.Request.Path == "/swagger/login.html" ||
                context.Request.Path == _options.SubmitUrl ||
                context.Request.Path == _options.CheckUrl)
            {
                await _next(context);
                return;
            }

            // 如果是API请求（如 fetch swagger.json），返回401
            if (context.Request.Path.Value.StartsWith("/swagger/v1/"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            // 否则，重定向到登录页面
            context.Response.Redirect("/swagger/login.html");
        }

        private bool IsAuthenticated(HttpContext context)
        {
            // 这里使用简单的Session验证，生产环境建议使用JWT
            return context.Session.TryGetValue("SwaggerAuth", out _);
        }
    }
}
