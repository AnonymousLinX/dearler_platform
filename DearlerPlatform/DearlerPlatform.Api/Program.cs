using System.Text;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Common.TokenModel.Models;
using DearlerPlatform.Core.Core;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Service;
using DearlerPlatform.Service.CustomerApp;
using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ShoppingCartApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<DealerPlatformContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped(typeof(LocalEventBus<>));
// 将AutoMapper注册到容器中, 并且添加实体映射类
builder.Services.AddAutoMapper(typeof(DearlerPlatformProfile));
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
builder.Services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
// 注入redis服务
var connectionString = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddSingleton(provider => new RedisCore(connectionString));
builder.Services.AddTransient<IRedisWorker, RedisWorker>();

var token = builder.Configuration.GetSection("Jwt").Get<JwtTokenModel>();
#region Jwt验证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    opt => {
        // development environment
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new (){
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Security)),
            ValidIssuer = token.Issuer,
            ValidAudience = token.Audience
        };
        opt.Events = new JwtBearerEvents
        {
            OnChallenge = context => {
                // 终止代码
                context.HandleResponse();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.WriteAsJsonAsync(new { code = 401, err = "无权限"});
                return Task.FromResult(0);
            }
        };
    }
);
#endregion
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() { Title = "DearlerPlatform", Version = "v1" });
    // 添加安全定义
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
        Description = "格式: Bearer {token}",
        Name = "Authorization", //默认的参数名
        In = ParameterLocation.Header, // 放于请求头中
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
                }
            }, new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(opt => 
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1")
    );
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("any");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
