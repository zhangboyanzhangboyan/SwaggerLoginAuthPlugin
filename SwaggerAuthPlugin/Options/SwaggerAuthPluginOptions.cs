namespace SwaggerAuthPlugin
{
    // 模型: 插件配置
    public class SwaggerAuthPluginOptions
    {
        /// <summary>是否启用登录授权</summary>
        public bool Enabled { get; set; } = true;
        /// <summary>检查登录状态的URL (必须是POST请求)</summary>
        public string CheckUrl { get; set; } = "/swagger/check-login";
        /// <summary>提交登录的URL (必须是POST请求)</summary>
        public string SubmitUrl { get; set; } = "/swagger/submit-login";
        /// <summary>生产环境是否自动开启</summary>
        public bool EnableOnProduction { get; set; } = true;
        /// <summary>默认用户名</summary>
        public string DefaultUsername { get; set; } = "admin";
        /// <summary>默认密码</summary>
        public string DefaultPassword { get; set; } = "1234567";
    }
}
