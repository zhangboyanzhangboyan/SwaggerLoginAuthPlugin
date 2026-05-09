using SwaggerAuthPlugin; // 必须引用！
using System.Reflection;  

var builder = WebApplication.CreateBuilder(args);

// ========== 1. 注册插件服务 ==========
//builder.Services.AddSwaggerAuth(options =>
//{
//    options.Enabled = true;
//    options.DefaultUsername = "admin";
//    options.DefaultPassword = "123456";
//});

builder.Services.AddSwaggerAuth(builder.Configuration.GetSection("SwaggerAuthPlugin"));

// ========== 2. 注册 Swagger 服务 ==========
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// -----

var app = builder.Build();

// ========== 3. 启用插件中间件（必须在 Swagger 之前） ==========
app.UseSwaggerAuth();

// ========== 4. 启用 Swagger 中间件 ==========
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// test
var assembly = Assembly.GetExecutingAssembly();   // 这里获取的是插件自身的程序集
var names = assembly.GetManifestResourceNames();
foreach (var name in names)
    Console.WriteLine(name);

app.Run();
