using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace SwaggerAuthPlugin
{
    public static class SwaggerAuthPluginExtensions
    {
        public static IServiceCollection AddSwaggerAuth(this IServiceCollection services, Action<SwaggerAuthPluginOptions>? configureOptions = null)
        {
            // 使用 Options 模式注册，支持从配置文件或委托进行配置
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            else
            {
                services.Configure<SwaggerAuthPluginOptions>(options => { });
            }

            services.AddDistributedMemoryCache();
            services.AddSession();

            // 可选：也注册一个单例，方便其他地方直接注入 SwaggerAuthPluginOptions
            //services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SwaggerAuthPluginOptions>>().Value);

            return services;
        }

        public static IApplicationBuilder UseSwaggerAuth(this IApplicationBuilder app)
        {
            // 启用Session中间件
            app.UseSession();
            // 启用认证中间件
            app.UseMiddleware<SwaggerAuthMiddleware>();

            // 使用物理文件提供程序，指向应用程序运行目录下的 Resources 文件夹
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new EmbeddedFileProvider(
                    Assembly.GetExecutingAssembly(),
                    baseNamespace: "SwaggerAuthPlugin.Resources"   // 用你实际获取的前缀替换
                ),
                RequestPath = "/swagger"
            });

            return app;
        }
    }
}
