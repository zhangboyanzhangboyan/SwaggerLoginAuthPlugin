using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SwaggerAuthPlugin
{
    [Route("swagger")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]   // 避免被全局授权中间件拦截
    public class SwaggerAuthController : ControllerBase
    {
        private readonly SwaggerAuthPluginOptions _options;

        public SwaggerAuthController(IOptions<SwaggerAuthPluginOptions> options)
        {
            _options = options.Value;
        }

        [HttpPost("submit-login")]
        public IActionResult SubmitLogin([FromBody] SwaggerLoginRequest request)
        {
            if (request.Username == _options.DefaultUsername &&
                request.Password == _options.DefaultPassword)
            {
                HttpContext.Session.SetString("SwaggerAuth", "true");
                return Ok(new SwaggerLoginResponse { Success = true, Message = "登录成功" });
            }

            // 返回 401 并携带错误详情
            return StatusCode(401, new SwaggerLoginResponse { Success = false, Message = "用户名或密码错误" });
        }

        [HttpPost("check-login")]
        public IActionResult CheckLogin()
        {
            if (HttpContext.Session.TryGetValue("SwaggerAuth", out _))
            {
                return Ok(new { authenticated = true });
            }

            return StatusCode(401, new { authenticated = false, message = "未登录" });
        }
    }
}
